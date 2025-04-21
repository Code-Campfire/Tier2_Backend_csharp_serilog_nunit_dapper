using CodeFire.Core.Interfaces;
using CodeFire.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodeFire.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly ITodoRepository _todoRepository;
    private readonly ILogger<TodoController> _logger;

    public TodoController(ITodoRepository todoRepository, ILogger<TodoController> logger)
    {
        _todoRepository = todoRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Todo>>> GetAll()
    {
        _logger.LogInformation("Getting all todos");
        var todos = await _todoRepository.GetAllAsync();
        return Ok(todos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Todo>> GetById(int id)
    {
        _logger.LogInformation("Getting todo with id {Id}", id);
        var todo = await _todoRepository.GetByIdAsync(id);
        
        if (todo == null)
        {
            _logger.LogWarning("Todo with id {Id} not found", id);
            return NotFound();
        }
        
        return Ok(todo);
    }

    [HttpPost]
    public async Task<ActionResult<Todo>> Create(Todo todo)
    {
        _logger.LogInformation("Creating a new todo");
        var id = await _todoRepository.CreateAsync(todo);
        todo.Id = id;
        
        return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Todo todo)
    {
        if (id != todo.Id)
        {
            _logger.LogWarning("Id mismatch: {PathId} vs {TodoId}", id, todo.Id);
            return BadRequest("Id mismatch");
        }
        
        var existingTodo = await _todoRepository.GetByIdAsync(id);
        
        if (existingTodo == null)
        {
            _logger.LogWarning("Todo with id {Id} not found for update", id);
            return NotFound();
        }
        
        _logger.LogInformation("Updating todo with id {Id}", id);
        var success = await _todoRepository.UpdateAsync(todo);
        
        if (!success)
        {
            _logger.LogError("Failed to update todo with id {Id}", id);
            return StatusCode(500, "Failed to update todo");
        }
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existingTodo = await _todoRepository.GetByIdAsync(id);
        
        if (existingTodo == null)
        {
            _logger.LogWarning("Todo with id {Id} not found for deletion", id);
            return NotFound();
        }
        
        _logger.LogInformation("Deleting todo with id {Id}", id);
        var success = await _todoRepository.DeleteAsync(id);
        
        if (!success)
        {
            _logger.LogError("Failed to delete todo with id {Id}", id);
            return StatusCode(500, "Failed to delete todo");
        }
        
        return NoContent();
    }
} 