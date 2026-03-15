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

        [HttpPost]
        public IActionResult CreateTask(TaskItem task)
        {
            task.Id = Guid.NewGuid();
            tasks.Add(task);
            return Ok(new CreatedTaskResponse { Id = task.Id });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTask(Guid id, TaskItem updatedTask)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            task.Title = updatedTask.Title;
            task.IsDone = updatedTask.IsDone;

            return Ok(task);
        }
    }
}
