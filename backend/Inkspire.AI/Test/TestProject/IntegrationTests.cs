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
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

public class CommentIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _output;

    public CommentIntegrationTests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;
        _client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor != null) services.Remove(descriptor);

                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("TestDatabase"));
            });
        }).CreateClient();

        _output = output;
    }

    [Fact]
    public async Task CreateComment_ShouldReturnSuccess_WhenValidRequest()
    {
        // Arrange
        var request = new CreateCommentCommand
        {
            Text = "testo testt",
            UserId = Guid.NewGuid(),
            BookId = Guid.NewGuid()
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        _output.WriteLine($"Sending POST request to create comment: {request.Text}");

        // Act
        var response = await _client.PostAsync("/api/Comment/create", jsonContent);

        Assert.True(response.IsSuccessStatusCode, $"Request failed with status code: {response.StatusCode}");

        var responseString = await response.Content.ReadAsStringAsync();
        _output.WriteLine($"Response: {responseString}");

        Assert.False(string.IsNullOrEmpty(responseString), "Response body is empty");

        var result = JsonSerializer.Deserialize<Result<CreateCommentResponse>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);

    }

    [Fact]
    public async Task GetCommentById_ShouldReturnComment_WhenCommentExists()
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

        using (var scope = _factory.Services.CreateScope()) 
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Comments.Add(comment);
            await dbContext.SaveChangesAsync();
        }

        _output.WriteLine($"Fetching comment with ID: {comment.Id}");

        // Act
        var response = await _client.GetAsync($"/api/comments/{comment.Id}");
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Result<GetCommentByIdQueryResponse>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        _output.WriteLine($"Response: {responseString}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(comment.Id, result.Value.CommentId);
        Assert.Equal(comment.Text, result.Value.Text);
    }

    [Fact]
    public async Task GetCommentById_ShouldReturnNotFound_WhenCommentDoesNotExist()
    {
        // Arrange
        var randomCommentId = Guid.NewGuid();
        _output.WriteLine($"Searching for non-existing comment ID: {randomCommentId}");

        // Act
        var response = await _client.GetAsync($"/api/comments/{randomCommentId}");
        var responseString = await response.Content.ReadAsStringAsync();

        _output.WriteLine($"Response: {responseString}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
