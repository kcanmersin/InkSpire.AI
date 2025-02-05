using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using Core.Service;

public class BookService : IBookService
{
    private readonly string _connectionString;
    private readonly ILogger<BookService> _logger;

    public BookService(IConfiguration configuration, ILogger<BookService> logger)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _logger = logger;
    }

    public async Task<List<PopularBook>> GetPopularBooksAsync(int limit)
    {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync(); 


                var books = await connection.QueryAsync<PopularBook>(
                    "GetPopularBooks",
                    new { limit_count = limit },
                    commandType: CommandType.StoredProcedure
                );

                return books.AsList();
            }
    }
}
