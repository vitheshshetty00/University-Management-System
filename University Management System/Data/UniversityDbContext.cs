using Microsoft.EntityFrameworkCore;
using University_Management_System.Entities;

namespace University_Management_System.Data
{
    public class UniversityDbContext(DbContextOptions<UniversityDbContext> options) : DbContext(options)
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<CreditCardPayment> CreditCardPayments { get; set; }
        public DbSet<BankTransferPayment> BankTransferPayments { get; set; }


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

            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new {sc.StudentId,sc.CourseId });

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId);

            modelBuilder.Entity<Payment>()
               .HasDiscriminator<string>("PaymentMethod")
               .HasValue<CreditCardPayment>("CreditCard")
               .HasValue<BankTransferPayment>("BankTransfer");

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Student)
                .WithMany(s => s.Payments)
                .HasForeignKey(p => p.StudentId);
        }
    }
}
