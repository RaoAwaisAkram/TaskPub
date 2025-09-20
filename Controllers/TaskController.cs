using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskApp.Models;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ModelContext _context;
    public TasksController(ModelContext context) => _context = context;

    [HttpPost]
[Authorize]
public async Task<IActionResult> CreateTask([FromBody] TaskCreateDto dto)
{
    var username = User.FindFirst(ClaimTypes.Name)?.Value;
    if (string.IsNullOrEmpty(username)) return Unauthorized();

    var task = new TaskModel
    {
        Title = dto.Title,
        Subject = dto.Subject,
        Isdone = dto.Isdone ?? false,
        Notificationon = dto.Notificationon ?? false,
        Starttime = dto.Starttime ?? DateTime.UtcNow,
        Createdby = username,
        Createdat = DateTime.UtcNow
    };

    _context.Tasks.Add(task);
    await _context.SaveChangesAsync();

    return Ok(task);
}


    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetTasks()
    {
        var username = User?.FindFirst(ClaimTypes.Name)?.Value ?? User?.Identity?.Name;
        if (string.IsNullOrEmpty(username)) return Unauthorized();

        var tasks = await _context.Tasks
            .Where(t => t.Createdby == username)
            .Select(t => new {
                t.Taskid, t.Title, t.Subject, t.Isdone, t.Createdat, t.Starttime, t.Notificationon
            })
            .ToListAsync();

        return Ok(tasks);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetTask(int id)
    {
        var username = User?.FindFirst(ClaimTypes.Name)?.Value ?? User?.Identity?.Name;
        var task = await _context.Tasks.FindAsync(id);
        if (task == null || task.Createdby != username) return NotFound();
        return Ok(task);
    }
    [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
}
