using Dapper;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using InkSpire.Core.Data.Entity;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.ElasticSearch
{
    public class ElasticsearchService : IElasticsearchService
    {
        readonly ElasticsearchClient _elasticsearchClient;
        private readonly string _connectionString;

        public ElasticsearchService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");

            var settings = new ElasticsearchClientSettings(new Uri(configuration["ElasticsearchSettings:Uri"]))
                .DefaultIndex(ElasticsearchIndexes.DefaultIndex);

            _elasticsearchClient = new ElasticsearchClient(settings);

        }
     


        public async Task<bool> SyncDatabaseToElasticsearch(CancellationToken cancellationToken = default)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var books = await connection.QueryAsync<Book>("SELECT * FROM Books");
            var tasks = books.Select(book => IndexBook(book));
            await Task.WhenAll(tasks);

            return true;
        }


        public async Task<bool> IndexBook(Book book)
        {
            var response = await _elasticsearchClient.IndexAsync(book, idx => idx.Index("books"));
            return response.IsValidResponse;
        }



        public async Task<bool> AddBook(Book book)
        {
            using var connection = new MySqlConnection(_connectionString);
            string query = "INSERT INTO Books (Id, Title, AuthorId, Content, Language, Level) VALUES (@Id, @Title, @AuthorId, @Content, @Language, @Level)";

            await connection.ExecuteAsync(query, book);
            return await IndexBook(book); 
        }

        public async Task<bool> CreateIndexAsync(string indexName = ElasticsearchIndexes.DefaultIndex, CancellationToken cancellationToken = default)
        {
            IndexResponse indexResponse = await _elasticsearchClient.IndexAsync(indexName, cancellationToken);

            return indexResponse.IsSuccess();
        }

        public async Task<bool> CreateDocumentAsync<T>(T document, string indexName = ElasticsearchIndexes.DefaultIndex, CancellationToken cancellationToken = default) where T : IElasticsearchModal
        {
            CreateRequest<T> createRequest = new(document, indexName, document.Id);
            CreateResponse createResponse = await _elasticsearchClient.CreateAsync(createRequest, cancellationToken);

            return createResponse.IsSuccess();
        }

        public async Task<bool> UpdateDocumentAsync<T>(string documentId, object partialDocument, string indexName = ElasticsearchIndexes.DefaultIndex, CancellationToken cancellationToken = default)
        {
            UpdateRequest<T, object> updateRequest = new(indexName, documentId)
            {
                Doc = partialDocument
            };
            UpdateResponse<T> updateResponse = await _elasticsearchClient.UpdateAsync<T, object>(updateRequest, cancellationToken);

            return updateResponse.IsSuccess();
        }

        public async Task<bool> DeleteDocumentAsync<T>(string documentId, string indexName = ElasticsearchIndexes.DefaultIndex, CancellationToken cancellationToken = default)
        {
            DeleteResponse deleteResponse = await _elasticsearchClient.DeleteAsync<T>(indexName, documentId, cancellationToken);

            return deleteResponse.IsSuccess();
        }

        public async Task<long> CountDocumentsAsync<T>(string indexName = ElasticsearchIndexes.DefaultIndex, CancellationToken cancellationToken = default) where T : IElasticsearchModal
        {
            CountResponse countResponse = await _elasticsearchClient.CountAsync<T>(indexName, cancellationToken);

            return countResponse.Count;
        }

        public async Task<IReadOnlyCollection<T>> SearchAsync<T>(Action<SearchRequestDescriptor<T>> searchRequestDescriptor, CancellationToken cancellationToken = default) where T : IElasticsearchModal
        {
            SearchResponse<T> searchResponse = await _elasticsearchClient.SearchAsync<T>(searchRequestDescriptor, cancellationToken);

            return searchResponse.Documents;
        }

        public async Task<IReadOnlyCollection<T>> GetDocumentsAsync<T>(string indexName = ElasticsearchIndexes.DefaultIndex, CancellationToken cancellationToken = default) where T : IElasticsearchModal
        {
            SearchResponse<T> searchResponse = await _elasticsearchClient.SearchAsync<T>(indexName, cancellationToken);

            return searchResponse.Documents;
        }

        public async Task<T> GetDocumentAsync<T>(string documentId, string indexName = ElasticsearchIndexes.DefaultIndex, CancellationToken cancellationToken = default) where T : IElasticsearchModal
        {
            GetResponse<T> getResponse = await _elasticsearchClient.GetAsync<T>(documentId, index => index
                                                                                                        .Index(indexName), cancellationToken);

            return getResponse.Source;
        }

        public async Task<IReadOnlyCollection<T>> MatchQueryAsync<T>(Expression<Func<T, object>> field, string queryKeyword, string indexName = ElasticsearchIndexes.DefaultIndex, CancellationToken cancellationToken = default, int from = 0, int size = 10) where T : IElasticsearchModal
        {
            SearchResponse<T> searchResponse = await _elasticsearchClient.SearchAsync<T>(index => index
                                                                                                    .Index(indexName)
                                                                                                    .Query(query => query
                                                                                                        .Match(t => t.Field(field)
                                                                                                                     .Query(queryKeyword)))
                                                                                                    .From(from)
                                                                                                    .Size(size), cancellationToken);

            return searchResponse.Documents;
        }

        public async Task<IReadOnlyCollection<T>> FuzzyQueryAsync<T>(Expression<Func<T, object>> field, string queryKeyword, string indexName = ElasticsearchIndexes.DefaultIndex, CancellationToken cancellationToken = default, int from = 0, int size = 10) where T : IElasticsearchModal
        {
            SearchResponse<T> searchResponse = await _elasticsearchClient.SearchAsync<T>(index => index
                                                                                                    .Index(indexName)
                                                                                                    .Query(query => query
                                                                                                        .Fuzzy(t => t.Field(field)
                                                                                                                     .Value(queryKeyword)))
                                                                                                    .From(from)
                                                                                                    .Size(size), cancellationToken);

            return searchResponse.Documents;
        }

        public async Task<IReadOnlyCollection<T>> WildcardQueryAsync<T>(Expression<Func<T, object>> field, string queryKeyword, string indexName = ElasticsearchIndexes.DefaultIndex, CancellationToken cancellationToken = default, int from = 0, int size = 10) where T : IElasticsearchModal
        {
            SearchResponse<T> searchResponse = await _elasticsearchClient.SearchAsync<T>(index => index
                                                                                                    .Index(indexName)
                                                                                                    .Query(query => query
                                                                                                        .Wildcard(t => t.Field(field)
                                                                                                                        .Value(queryKeyword)))
                                                                                                    .From(from)
                                                                                                    .Size(size), cancellationToken);

            return searchResponse.Documents;
        }

        public async Task<IReadOnlyCollection<T>> BoolQueryAsync<T>(Expression<Func<T, object>> matchField, string matchQueryKeyword, Expression<Func<T, object>> fuzzyField, string fuzzyQueryKeyword, Expression<Func<T, object>> wildcardField, string wildcardQueryKeyword, string indexName = ElasticsearchIndexes.DefaultIndex, CancellationToken cancellationToken = default, int from = 0, int size = 10) where T : IElasticsearchModal
        {
            SearchResponse<T> searchResponse = await _elasticsearchClient.SearchAsync<T>(index => index
                                                                                                    .Index(indexName)
                                                                                                    .Query(query => query
                                                                                                        .Bool(t => t.Should(
                                                                                                            match => match.Match(t => t.Field(matchField).Query(matchQueryKeyword)),
                                                                                                            fuzzy => fuzzy.Fuzzy(p => p.Field(fuzzyField).Value(fuzzyQueryKeyword)),
                                                                                                            wildcard => wildcard.Wildcard(p => p.Field(wildcardField).Value(wildcardQueryKeyword))
                                                                                                            )))
                                                                                                    .From(from)
                                                                                                    .Size(size), cancellationToken);

            return searchResponse.Documents;
        }

        public async Task<IReadOnlyCollection<T>> TermQueryAsync<T>(Expression<Func<T, object>> field, string queryKeyword, string indexName = ElasticsearchIndexes.DefaultIndex, CancellationToken cancellationToken = default, int from = 0, int size = 10) where T : IElasticsearchModal
        {
            SearchResponse<T> searchResponse = await _elasticsearchClient.SearchAsync<T>(index => index
                                                                                            .Index(indexName)
                                                                                            .Query(query => query
                                                                                                .Term(t => t.Field(field)
                                                                                                            .Value(queryKeyword)))
                                                                                            .From(from)
                                                                                            .Size(size), cancellationToken);

            return searchResponse.Documents;
        }

        public async Task<IReadOnlyCollection<T>> ExistsQueryAsync<T>(Expression<Func<T, object>> field, string indexName = ElasticsearchIndexes.DefaultIndex, CancellationToken cancellationToken = default, int from = 0, int size = 10) where T : IElasticsearchModal
        {
            SearchResponse<T> searchResponse = await _elasticsearchClient.SearchAsync<T>(index => index
                                                                                            .Index(indexName)
                                                                                            .Query(query => query
                                                                                                .Exists(t => t.Field(field)))
                                                                                            .From(from)
                                                                                            .Size(size), cancellationToken);

            return searchResponse.Documents;
        }


    }
}
