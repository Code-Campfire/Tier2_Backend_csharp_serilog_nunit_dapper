using CodeFire.Core.Models;

namespace CodeFire.Core.Interfaces;

public interface ITodoRepository
{
    Task<IEnumerable<Todo>> GetAllAsync();
    Task<Todo?> GetByIdAsync(int id);
    Task<int> CreateAsync(Todo todo);
    Task<bool> UpdateAsync(Todo todo);
    Task<bool> DeleteAsync(int id);
} 