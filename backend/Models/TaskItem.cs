namespace backend.Models
{
    public class TaskItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public bool IsDone { get; internal set; }
    }
}

namespace backend.Models
    {
        public class CreatedTaskResponse
        {
            public Guid Id { get; set; }
        }
    }  