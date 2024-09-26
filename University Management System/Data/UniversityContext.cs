using Microsoft.EntityFrameworkCore;
using University_Management_System.Entities;

namespace University_Management_System.Data
{
    public class UniversityContext(DbContextOptions<UniversityContext> options) : DbContext(options)
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
            .OwnsOne(s => s.Address);

            modelBuilder.Entity<Faculty>()
                .OwnsOne(f => f.Address);


            modelBuilder.Entity<Faculty>()
                .HasMany(f => f.CoursesTaught)
                .WithOne(c => c.Faculty)
                .HasForeignKey(c => c.FacultyId);
        }
    }
}
