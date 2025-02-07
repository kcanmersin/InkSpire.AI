using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.ElasticSearch;
using InkSpire.Core.Data.Entity;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elastic.Clients.Elasticsearch;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IElasticsearchService _elasticService;

        public SearchController(IElasticsearchService elasticService)
        {
            _elasticService = elasticService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks(
            [FromQuery] string? title = null,
            [FromQuery] string? authorId = null,
            [FromQuery] string? content = null,
            [FromQuery] string? language = null,
            [FromQuery] string? level = null,
            [FromQuery] int from = 0,
            [FromQuery] int size = 10,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrEmpty(title) &&
                    string.IsNullOrEmpty(authorId) &&
                    string.IsNullOrEmpty(content) &&
                    string.IsNullOrEmpty(language) &&
                    string.IsNullOrEmpty(level))
                {
                    var allBooks = await _elasticService.GetDocumentsAsync<Book>("books", cancellationToken);
                    return Ok(allBooks);
                }

                IReadOnlyCollection<Book> searchResults;

                if (!string.IsNullOrEmpty(title))
                {
                    searchResults = await _elasticService.FuzzyQueryAsync<Book>(
                        field: b => b.Title, 
                        queryKeyword: title,
                        indexName: "books",
                        cancellationToken: cancellationToken,
                        from: from,
                        size: size
                    );
                }
                else
                {
                    var mustQueries = new List<Query>();

                    if (!string.IsNullOrEmpty(authorId))
                        mustQueries.Add(new MatchQuery(new Field("authorId.keyword")) { Query = authorId });

                    if (!string.IsNullOrEmpty(content))
                        mustQueries.Add(new MatchQuery(new Field("content")) { Query = content });

                    if (!string.IsNullOrEmpty(language))
                        mustQueries.Add(new MatchQuery(new Field("language.keyword")) { Query = language });

                    if (!string.IsNullOrEmpty(level))
                        mustQueries.Add(new MatchQuery(new Field("level.keyword")) { Query = level });

                    searchResults = await _elasticService.SearchAsync<Book>(s => s
                        .Index("books")
                        .Query(q => q.Bool(b => b.Must(mustQueries))) 
                        .From(from)
                        .Size(size),
                        cancellationToken
                    );
                }

                return Ok(searchResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Arama sırasında hata oluştu", details = ex.Message });
            }
        }



    }
}
