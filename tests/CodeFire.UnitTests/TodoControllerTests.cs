using CodeFire.API.Controllers;
using CodeFire.Core.Interfaces;
using CodeFire.Core.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace CodeFire.UnitTests;

[TestFixture]
public class TodoControllerTests
{
    private Mock<ITodoRepository> _mockRepository = null!;
    private Mock<ILogger<TodoController>> _mockLogger = null!;
    private TodoController _controller = null!;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<ITodoRepository>();
        _mockLogger = new Mock<ILogger<TodoController>>();
        _controller = new TodoController(_mockRepository.Object, _mockLogger.Object);
    }

    [Test]
    public async Task GetAll_ReturnsOkResult_WithListOfTodos()
    {
        // Arrange
        var todos = new List<Todo>
        {
            new Todo { Id = 1, Title = "Test Todo 1" },
            new Todo { Id = 2, Title = "Test Todo 2" }
        };
        
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(todos);
        
        // Act
        var result = await _controller.GetAll();
        
        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult?.Value.Should().BeEquivalentTo(todos);
    }

    [Test]
    public async Task GetById_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var todo = new Todo { Id = 1, Title = "Test Todo" };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(todo);
        
        // Act
        var result = await _controller.GetById(1);
        
        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult?.Value.Should().BeEquivalentTo(todo);
    }

    [Test]
    public async Task GetById_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Todo?)null);
        
        // Act
        var result = await _controller.GetById(999);
        
        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task Create_ValidTodo_ReturnsCreatedAtAction()
    {
        // Arrange
        var todo = new Todo { Title = "New Todo" };
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Todo>())).ReturnsAsync(1);
        
        // Act
        var result = await _controller.Create(todo);
        
        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result.Result as CreatedAtActionResult;
        
        createdResult?.ActionName.Should().Be(nameof(TodoController.GetById));
        createdResult?.RouteValues.Should().ContainKey("id").And.ContainValue(1);
        
        var createdTodo = createdResult?.Value as Todo;
        createdTodo.Should().NotBeNull();
        createdTodo?.Id.Should().Be(1);
    }

    [Test]
    public async Task Update_ValidTodo_ReturnsNoContent()
    {
        // Arrange
        var todo = new Todo { Id = 1, Title = "Updated Todo" };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Todo { Id = 1 });
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Todo>())).ReturnsAsync(true);
        
        // Act
        var result = await _controller.Update(1, todo);
        
        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Test]
    public async Task Update_InvalidId_ReturnsBadRequest()
    {
        // Arrange
        var todo = new Todo { Id = 1, Title = "Updated Todo" };
        
        // Act
        var result = await _controller.Update(2, todo);
        
        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task Delete_ValidId_ReturnsNoContent()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Todo { Id = 1 });
        _mockRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);
        
        // Act
        var result = await _controller.Delete(1);
        
        // Assert
        result.Should().BeOfType<NoContentResult>();
    }
} 