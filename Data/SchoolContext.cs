using Microsoft.EntityFrameworkCore;
using SchoolApi.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Student> Students { get; set; }
    public DbSet<Lecturer> Lecturers { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseStudent> CourseStudents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CourseStudent>()
            .HasKey(cs => new { cs.StudentId, cs.CourseId });

        modelBuilder.Entity<CourseStudent>()
            .HasOne(cs => cs.Student)
            .WithMany(s => s.CourseStudents)
            .HasForeignKey(cs => cs.StudentId);

        modelBuilder.Entity<CourseStudent>()
            .HasOne(cs => cs.Course)
            .WithMany(c => c.CourseStudents)
            .HasForeignKey(cs => cs.CourseId);
    }
}
