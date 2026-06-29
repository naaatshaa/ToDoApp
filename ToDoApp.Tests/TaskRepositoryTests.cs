using Microsoft.EntityFrameworkCore;
using ToDoApp.Data;
using ToDoApp.Models;
using Xunit;

namespace ToDoApp.Tests;

public class TaskRepositoryTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task AddAsync_ShouldAddTask()
    {
        var context = GetDbContext();
        var repository = new TaskRepository(context);
        var task = new TaskItem { Title = "Тестовая задача" };

        await repository.AddAsync(task);
        var all = await repository.GetAllAsync();
        
        Assert.Single(all);
        Assert.Equal("Тестовая задача", all[0].Title);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnTask()
    {
        var context = GetDbContext();
        var repository = new TaskRepository(context);
        var task = new TaskItem { Title = "Найти меня" };
        await repository.AddAsync(task);

        var found = await repository.GetByIdAsync(task.Id);

        Assert.NotNull(found);
        Assert.Equal("Найти меня", found.Title);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveTask()
    {
        var context = GetDbContext();
        var repository = new TaskRepository(context);
        var task = new TaskItem { Title = "Удалить меня" };
        await repository.AddAsync(task);

        await repository.DeleteAsync(task.Id);
        var all = await repository.GetAllAsync();

        Assert.Empty(all);
    }
}
