using Xunit;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Data;
using Core.Shared;
using Core.Features.CommentFeatures.Commands.CreateComment;
using Core.Features.CommentFeatures.Queries.GetCommentById;
using InkSpire.Core.Data.Entity;
using System.Collections.Generic;
using Xunit.Abstractions;

public class CommentHandlerTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly Mock<IValidator<CreateCommentCommand>> _mockValidator;
    private readonly CreateCommentHandler _createHandler;

    private readonly GetCommentByIdQueryHandler _getHandler;
    private readonly ITestOutputHelper _output;

    public CommentHandlerTests(ITestOutputHelper output)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _mockValidator = new Mock<IValidator<CreateCommentCommand>>();
        _output = output;

        _createHandler = new CreateCommentHandler(_dbContext, _mockValidator.Object);
        _getHandler = new GetCommentByIdQueryHandler(_dbContext);
    }

    [Fact]
    public async Task Handle_CreateComment_ShouldReturnSuccess_WhenValidRequest()
    {
        // Arrange
        var request = new CreateCommentCommand
        {
            Text = "testo testt",
            UserId = Guid.NewGuid(),
            BookId = Guid.NewGuid()
        };

        _output.WriteLine($"Creating comment with Text {request.Text}, UserId: {request.UserId}, BookId: {request.BookId}");

        var validationResult = new FluentValidation.Results.ValidationResult();

        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _createHandler.Handle(request, CancellationToken.None);
        _output.WriteLine($"Result Success: {result.IsSuccess}, Comment ID: {result.Value?.Id}, Text: {result.Value?.Text}");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
        Assert.Equal("testo testt", result.Value.Text);
        Assert.Equal(request.UserId, result.Value.UserId);
        Assert.Equal(request.BookId, result.Value.BookId);

        var dbComment = await _dbContext.Comments.FindAsync(result.Value.Id);
        Assert.NotNull(dbComment);
        Assert.Equal(request.Text, dbComment.Text);
    }

    [Fact]
    public async Task Handle_GetCommentById_ShouldReturnComment_WhenCommentExists()
    {
        // Arrange
        var comment = new Comment
        {
            Id = Guid.NewGuid(),
            Text = "Existing comment",
            UserId = Guid.NewGuid(),
            BookId = Guid.NewGuid(),
            Reactions = new List<Reaction>()
        };

        _output.WriteLine($"Adding comment to DB: ID={comment.Id}, Text={comment.Text}");

        _dbContext.Comments.Add(comment);
        await _dbContext.SaveChangesAsync();


        var request = new GetCommentByIdQuery { CommentId = comment.Id };

        // Act
        var result = await _getHandler.Handle(request, CancellationToken.None);

        _output.WriteLine($"Fetched Comment: ID={result.Value?.CommentId}, Text={result.Value?.Text}");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(comment.Id, result.Value.CommentId);
        Assert.Equal(comment.Text, result.Value.Text);
        Assert.Equal(comment.UserId, result.Value.UserId);
        Assert.Equal(comment.BookId, result.Value.BookId);
        Assert.Empty(result.Value.Reactions);
    }

}
