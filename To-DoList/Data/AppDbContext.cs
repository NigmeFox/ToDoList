using Microsoft.EntityFrameworkCore;
namespace To_Do_List.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<TaskItem> TaskItems { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.HasKey(e => e.ID);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.Description)
                .HasMaxLength(500);
                
            entity.Property(e => e.Status)
                .HasConversion<string>()
                .IsRequired();
            
            entity.Property(e => e.StartDate)
                .HasColumnType("date");
            
            entity.Property(e => e.EndDate)
                .HasColumnType("date");
        });
    }
}