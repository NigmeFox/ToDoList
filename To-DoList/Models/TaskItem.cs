using Microsoft.EntityFrameworkCore;

namespace To_Do_List;

public class TaskItem
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public TaskStatus Status { get; set; }
    
    private bool IsOverude => Status != TaskStatus.Completed && DateTime.Now >= EndDate;
}