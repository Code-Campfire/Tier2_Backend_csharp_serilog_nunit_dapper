using CodeFire.Core.Interfaces;
using CodeFire.Core.Models;
using CodeFire.Infrastructure.Data;
using Dapper;
using Microsoft.Extensions.Logging;

namespace CodeFire.Infrastructure.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<TodoRepository> _logger;

    public TodoRepository(IDbConnectionFactory connectionFactory, ILogger<TodoRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<IEnumerable<Todo>> GetAllAsync()
    {
        _logger.LogInformation("Getting all todos");
        using var connection = await _connectionFactory.CreateConnectionAsync();
        
        return await connection.QueryAsync<Todo>("SELECT * FROM Todos");
    }

    public async Task<Todo?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Getting todo with Id {Id}", id);
        using var connection = await _connectionFactory.CreateConnectionAsync();
        
        return await connection.QuerySingleOrDefaultAsync<Todo>(
            "SELECT * FROM Todos WHERE Id = @Id", 
            new { Id = id });
    }

    public async Task<int> CreateAsync(Todo todo)
    {
        _logger.LogInformation("Creating new todo with title {Title}", todo.Title);
        using var connection = await _connectionFactory.CreateConnectionAsync();
        
        todo.CreatedAt = DateTime.UtcNow;
        
        var id = await connection.ExecuteScalarAsync<int>(@"
            INSERT INTO Todos (Title, Description, IsCompleted, CreatedAt, CompletedAt) 
            VALUES (@Title, @Description, @IsCompleted, @CreatedAt, @CompletedAt);
            SELECT last_insert_rowid();", todo);
        
        _logger.LogInformation("Created todo with Id {Id}", id);
        return id;
    }

    public async Task<bool> UpdateAsync(Todo todo)
    {
        _logger.LogInformation("Updating todo with Id {Id}", todo.Id);
        using var connection = await _connectionFactory.CreateConnectionAsync();
        
        // If marked as completed, set CompletedAt
        if (todo.IsCompleted && !todo.CompletedAt.HasValue)
        {
            todo.CompletedAt = DateTime.UtcNow;
        }
        
        var result = await connection.ExecuteAsync(@"
            UPDATE Todos SET 
                Title = @Title, 
                Description = @Description, 
                IsCompleted = @IsCompleted, 
                CompletedAt = @CompletedAt
            WHERE Id = @Id", todo);
        
        return result > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("Deleting todo with Id {Id}", id);
        using var connection = await _connectionFactory.CreateConnectionAsync();
        
        var result = await connection.ExecuteAsync("DELETE FROM Todos WHERE Id = @Id", new { Id = id });
        
        return result > 0;
    }
} 