namespace To_Do_List.Interfaces;

public interface ITaskManager
{
    public Task AddTask();
    public Task<List<TaskItem>> GetAllTasks();
    public Task GetTaskById(int id);
    public Task ChangeTaskStatus(int id, TaskStatus newStatus);
    public Task UpdateTask(int id, string newName, string description, TaskStatus newStatus);
    public Task DeleteTask(int id);
}