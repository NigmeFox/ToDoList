using Microsoft.EntityFrameworkCore;
using To_Do_List.Data;
using To_Do_List.Interfaces;
using To_Do_List.Services;
using TaskStatus = To_Do_List.TaskStatus;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
        config.AddJsonFile("appsetting.json", optional: true, reloadOnChange: true))
    .ConfigureServices((context, services) =>
    {
        var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<ITaskManager, TaskManager>();
    })
    .Build();

bool running = true;
var manager = host.Services.GetRequiredService<ITaskManager>();

while (running)
{
    running = ShowMenu();
}

bool ShowMenu()
{
    Console.Clear();
    Console.WriteLine("To Do List");
    Console.WriteLine("1. Add Task");
    Console.WriteLine("2. Get All Tasks");
    Console.WriteLine("3. Update Task");
    Console.WriteLine("4. Delete Task");
    Console.WriteLine("5. Change status Task");
    Console.WriteLine("6. Exit");
    
    var input = Console.ReadLine();
    
    switch (input)
    {
        case "1":
            manager.AddTask();
            break;
        case "2":
            manager.GetAllTasks();
            break;
        case "3":
            Console.WriteLine("Enter Task Id");
            var id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Task Name");
            var name = Console.ReadLine();
            Console.WriteLine("Enter Task Description");
            var description = Console.ReadLine();
            Console.WriteLine("Enter Task Status");
            Console.WriteLine("1. Created,\n    2.Completed,\n    3.Canceled,\n    4.InProgress,\n    5.Overdue");
            var status = Enum.Parse<TaskStatus>(Console.ReadLine());
            manager.UpdateTask(id, name, description, status);
            break;
        case "4":
            Console.WriteLine("Enter Task Id");
            var idDelete = int.Parse(Console.ReadLine());
            manager.DeleteTask(idDelete);
            break;
        case "5":
            Console.WriteLine("Enter Task Id");
            var idUpdate = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Task Status");
            Console.WriteLine("1. Created,\n    2.Completed,\n    3.Canceled,\n    4.InProgress,\n    5.Overdue");
            var statusTask = Enum.Parse<TaskStatus>(Console.ReadLine());
            manager.ChangeTaskStatus(idUpdate, statusTask);
            break;
        case "6":
            return false;
        default:
            Console.WriteLine("Invalid Input");
            return false;
    }
    return true;
}