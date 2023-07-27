using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;
using TodoApi.ViewModels;


namespace TodoApi.Controllers;

[ApiController]
[Route("v1")]
public class TodosController : ControllerBase
{
    [HttpGet]
    [Route("todos")]
    public async Task<IActionResult> GetAllAsync([FromServices] AppDbContext context)
    {
        var todos = await
                    context.
                    Todos.
                    AsNoTracking().
                    ToListAsync();

        return Ok(todos);
    }

    [HttpGet]
    [Route("todos/{id}")]
    public async Task<IActionResult> GetByIdAsync([FromServices] AppDbContext context, [FromRoute] int id)
    {
        var todo = await
            context.
            Todos.
            AsNoTracking().
            FirstOrDefaultAsync(T => T.Id == id);

        return todo == null ? NotFound() : Ok(todo);
    }

    [HttpPost]
    [Route("todos")]
    public async Task<IActionResult> PostAsync([FromServices] AppDbContext context, [FromBody] CreateTodoViewModel todoViewModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var todo = new Todo
        {
            Title = todoViewModel.Title,
            Done = false,
            Date = DateTime.Now
        };
        try
        {
            await context.Todos.AddAsync(todo);
            await context.SaveChangesAsync();
            return Created($"/v1/todos/{todo.Id}", todo);
            // return CreatedAtAction(nameof(GetByIdAsync), new { id = todo.Id }, todo); Não sei pq não está funcionando
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    [HttpPut]
    [Route("todos/{id}")]
    public async Task<IActionResult> PutAsync([FromServices] AppDbContext context, [FromRoute] int id, [FromBody] CreateTodoViewModel todoViewModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var todo = await context.Todos.FirstOrDefaultAsync(T => T.Id == id);

        if (todo == null)
            return NotFound();

        todo.Title = todoViewModel.Title;

        try
        {
            context.Todos.Update(todo);
            await context.SaveChangesAsync();
            return Ok(todo);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete]
    [Route("todos/{id}")]
    public async Task<IActionResult> DeleteAsync([FromServices] AppDbContext context, [FromRoute] int id)
    {
        var todo = await context.Todos.FirstOrDefaultAsync(T => T.Id == id);

        if (todo == null)
            return NotFound();

        try
        {
            context.Todos.Remove(todo);
            await context.SaveChangesAsync();
            return Ok(todo);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch]
    [Route("todos/{id}/done")]
    public async Task<IActionResult> PatchAsync([FromServices] AppDbContext context, [FromRoute] int id)
    {
        var todo = await context.Todos.FirstOrDefaultAsync(T => T.Id == id);

        if (todo == null)
            return NotFound();

        todo.Done = true;

        try
        {
            context.Todos.Update(todo);
            await context.SaveChangesAsync();
            return Ok(todo);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch]
    [Route("todos/{id}/undone")]
    public async Task<IActionResult> PatchUndoneAsync([FromServices] AppDbContext context, [FromRoute] int id)
    {
        var todo = await context.Todos.FirstOrDefaultAsync(T => T.Id == id);

        if (todo == null)
            return NotFound();

        todo.Done = false;

        try
        {
            context.Todos.Update(todo);
            await context.SaveChangesAsync();
            return Ok(todo);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
