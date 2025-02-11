using Core.Data;
using Core.Shared;
using FluentValidation;
using InkSpire.Core.Data.Entity;
using MediatR;
using InkSpire.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.ElasticSearch;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Core.Features.BookFeatures.Commands.CreateBook
{
    public class CreateBookHandler : IRequestHandler<CreateBookCommand, Result<CreateBookResponse>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<CreateBookCommand> _validator;
        private readonly IGroqLLM _groqLLMService;
        private readonly IImageGenerationService _imageGenerationService;
        private readonly IElasticsearchService _elasticsearchService;
        private readonly IConnection _rabbitMqConnection;

        public CreateBookHandler(
            ApplicationDbContext context,
            IValidator<CreateBookCommand> validator,
            IGroqLLM groqLLMService,
            IImageGenerationService imageGenerationService,
            IElasticsearchService elasticsearchService,
            IConnection rabbitMqConnection)
        {
            _context = context;
            _validator = validator;
            _groqLLMService = groqLLMService;
            _imageGenerationService = imageGenerationService;
            _elasticsearchService = elasticsearchService;
            _rabbitMqConnection = rabbitMqConnection;
        }

        public async Task<Result<CreateBookResponse>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Failure<CreateBookResponse>(
                    new Error("ValidationFailed", validationResult.Errors.First().ErrorMessage));
            }

            var generatedContent = await _groqLLMService.GenerateContentAsync(
                title: request.Title,
                language: request.Language,
                level: request.Level
            );

            var book = new Book(
                authorId: request.AuthorId,
                title: request.Title,
                content: generatedContent,
                language: request.Language,
                level: request.Level
            );

            await _context.Books.AddAsync(book, cancellationToken);

            //var imageIdeas = await _groqLLMService.GetImageIdeasAsync(book.Content);
            //if (imageIdeas == null || imageIdeas.Count == 0)
            //{
            //    imageIdeas = new List<string>
            //    {
            //        $"Cover art for {request.Title}",
            //        "Interior illustration #1",
            //        "Interior illustration #2"
            //    };
            //}

            //foreach (var idea in imageIdeas)
            //{
            //    try
            //    {
            //        var imageBytes = await _imageGenerationService.GenerateImageAsync(idea);
            //        if (imageBytes != null && imageBytes.Length > 0)
            //        {
            //            var bookImage = new BookImage(book.Id, imageBytes);
            //            await _context.BookImages.AddAsync(bookImage, cancellationToken);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Image generation failed for idea '{idea}': {ex.Message}");
            //    }
            //}
            //sleep 10 second
            await Task.Delay(3000);
            await _context.SaveChangesAsync(cancellationToken);

            var generatedTest = await _groqLLMService.GenerateTestAsync(
               book: book,
               user: null,
               level: book.Level,
               content: book.Content,
               language: book.Language
           );

            if (generatedTest.Questions != null && generatedTest.Questions.Any())
            {
                generatedTest.UserId = null;
                generatedTest.BookId = book.Id;

                await _context.Test.AddAsync(generatedTest, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
            await _elasticsearchService.CreateDocumentAsync(book);

            PublishBookCreatedEvent(book);

            var response = new CreateBookResponse
            {
                BookId = book.Id,
                Title = book.Title
            };

            return Result.Success(response);
        }

        private void PublishBookCreatedEvent(Book book)
        {
            using var channel = _rabbitMqConnection.CreateModel();
            channel.QueueDeclare(queue: "bookcreated", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var bookEvent = new { BookId = book.Id, Title = book.Title };
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(bookEvent));

            channel.BasicPublish(exchange: "", routingKey: "bookcreated", basicProperties: null, body: body);
        }
    }
}
