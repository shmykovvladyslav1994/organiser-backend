using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("tasks")]
    public class TasksController : Controller
    {
        private static List<TaskItem> tasks = new List<TaskItem>();

        [HttpGet]
        public IEnumerable<TaskItem> GetTasks()
        {
            return tasks;
        }

        [HttpGet("{id}")]
        public IActionResult GetTask(Guid id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpPost]
        public IActionResult CreateTask(TaskItem task)
        {
            task.Id = Guid.NewGuid();
            tasks.Add(task);
            return Ok(new CreatedTaskResponse { Id = task.Id });
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateTask(Guid id, UpdateTaskDto updatedTask)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            task.Title = updatedTask.Title ?? task.Title;
            task.IsDone = updatedTask.IsDone ?? task.IsDone;

            return Ok(task);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(Guid id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            tasks.Remove(task);

            return NoContent();
        }

        [HttpDelete]
        public IActionResult DeleteAllTasks()
        {
            tasks.Clear();
            return NoContent();
        }
    }
}
