using Microsoft.EntityFrameworkCore;
using To_Do_List.Data;
using To_Do_List.Interfaces;

namespace To_Do_List.Services;

public class TaskManager : ITaskManager
{
    private readonly AppDbContext _appDbContext;
    private int lastId = 0;

    public TaskManager(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task AddTask()
    {
        Console.WriteLine("Name of the task");
        var name = Console.ReadLine();
        
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Task name cannot be empty!");
            return;
        }
        
        Console.WriteLine("Description of the task");
        var description = Console.ReadLine();
        
        var newTask = new TaskItem
        {
            Name = name,
            Description = description,
            StartDate = DateTime.Now,
            Status = TaskStatus.Created
        };
        await _appDbContext.AddAsync(newTask);
        await _appDbContext.SaveChangesAsync();
        Console.WriteLine("Task created");
    }

    public async Task<List<TaskItem>> GetAllTasks()
    {
        var taskItems = await _appDbContext.TaskItems.AsNoTracking().ToListAsync();
        foreach (var taskItem in taskItems)
        {
            Console.WriteLine(taskItem.ID);
            Console.WriteLine(taskItem.Name);
            Console.WriteLine(taskItem.Description);
            Console.WriteLine(taskItem.StartDate);
            Console.WriteLine(taskItem.Status);
            Console.WriteLine(taskItem.EndDate);
        }
        return taskItems;
    }

    public async Task GetTaskById(int taskId)
    {
        var taskItem = await _appDbContext.TaskItems.FindAsync(taskId);

        if (taskItem == null)
        {
            Console.WriteLine($"Task with ID {taskId} not found.");
            return;
        }

        Console.WriteLine(
            $"Task {taskItem.ID}: {taskItem.Name}\n" +
            $"Description: {taskItem.Description}\n" +
            $"Status: {taskItem.Status}\n" +
            $"Start Date: {taskItem.StartDate}\n" +
            $"End Date: {taskItem.EndDate}"
        );
    }

    public async Task ChangeTaskStatus(int taskId, TaskStatus newStatus)
    {
        var taskItem = await _appDbContext.TaskItems.FindAsync(taskId);
        
        if (taskItem == null)
        {
            Console.WriteLine($"Task with ID {taskId} not found.");
            return;
        }
        
        taskItem.Status = newStatus;
        await _appDbContext.SaveChangesAsync();
        Console.WriteLine("Task status updated");
    }

    public async Task UpdateTask(int taskId ,string newName, string newDescription, TaskStatus newStatus)
    {
        if (!Enum.IsDefined(typeof(TaskStatus), newStatus))
        {
            throw new ArgumentException($"Invalid task status: {newStatus}");
        }
        
        var task = await _appDbContext.TaskItems.FindAsync(taskId);
        if (task == null)
        {
            Console.WriteLine($"Task with ID {taskId} not found.");
            return;
        }
        
        task.Status = newStatus;
        task.Description = newDescription;
        task.Name = newName;
        await _appDbContext.SaveChangesAsync();
        Console.WriteLine("Task updated");
    }

    public async Task DeleteTask(int taskId)
    {
        var task = await _appDbContext.TaskItems.FindAsync(taskId);
        if (task == null)
        {
            Console.WriteLine($"Task {taskId} not found");
            return;
        }
        _appDbContext.TaskItems.Remove(task);
        await _appDbContext.SaveChangesAsync();
        Console.WriteLine("Task deleted");
    }
}