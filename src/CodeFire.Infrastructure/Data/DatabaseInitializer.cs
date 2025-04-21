using Dapper;
using Serilog;

namespace CodeFire.Infrastructure.Data;

public class DatabaseInitializer
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DatabaseInitializer(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task InitializeAsync()
    {
        try
        {
            // Create database tables
            using var connection = await _connectionFactory.CreateConnectionAsync();
            
            // Create Todo table
            await connection.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS Todos (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Description TEXT,
                    IsCompleted INTEGER NOT NULL,
                    CreatedAt TEXT NOT NULL,
                    CompletedAt TEXT
                );
            ");

            Log.Information("Database initialized successfully");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error initializing database");
            throw;
        }
    }
} 