using Microsoft.AspNetCore.Mvc;
using ToDoApp.Models;
using ToDoApp.Data;

namespace ToDoApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly TaskRepository _repository;

    public TasksController(TaskRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _repository.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await _repository.GetByIdAsync(id);
        return task is null ? NotFound() : Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TaskItem task)
    {
        if (string.IsNullOrWhiteSpace(task.Title))
            return BadRequest("Title is required");

        await _repository.AddAsync(task);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TaskItem updated)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null)
            return NotFound();

        existing.Title = updated.Title;
        existing.Description = updated.Description;
        existing.IsCompleted = updated.IsCompleted;

        await _repository.UpdateAsync(existing);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null)
            return NotFound();

        await _repository.DeleteAsync(id);
        return NoContent();
    }
}