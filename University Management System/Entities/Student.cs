using System;
using System.ComponentModel.DataAnnotations;

namespace University_Management_System.Entities
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public DateTime EnrollmentDate { get; set; }

        public Address? Address { get; set; }

        public Decimal? Fees { get; set; }
        public bool PaymentStatus { get; set; }
        public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();

        public List<Payment>? Payments { get; set; }


        public virtual decimal CalculateFees()
        {
            return 1000;
        }
        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Email: {Email}, Date of Birth: {DateOfBirth.ToShortDateString()}, Enrollment Date: {EnrollmentDate.ToShortDateString()}, Address: {Address}";
        }
    }

    class FullTimeStudent : Student
    {
        public override decimal CalculateFees()
        {
            int courseCount = StudentCourses.Count;
            decimal amountPerCredit = 1000;
            decimal totalAmount = 15000;
            Console.WriteLine(ToString());
            Console.WriteLine();

            Console.WriteLine($"Student Type: Full Time Student");
            Console.WriteLine($"Number of Courses: {courseCount}");
            Console.WriteLine($"Amount per Credit: {amountPerCredit} Rupees");
            Console.WriteLine($"Miscellaneous Fee: 15000 Rupees");
            

            foreach(var studentCourse in StudentCourses)
            {
                
                decimal amt= studentCourse.Course.Credits * amountPerCredit;
                Console.WriteLine($"{studentCourse.Course.Name} : {studentCourse.Course.Credits} * {amountPerCredit} = {amt} Rupees " );
                totalAmount += amt;
            }

            Console.WriteLine($"Total Amount:{totalAmount} Rupees");

            return totalAmount;
        }
    }

    class PartTimeStudent : Student
    {
        public override decimal CalculateFees()
        {
            int courseCount = StudentCourses.Count;
            decimal amountPerCredit = 500;
            decimal totalAmount = 5000;
            Console.WriteLine(ToString());
            Console.WriteLine();

            Console.WriteLine($"Student Type: Full Time Student");
            Console.WriteLine($"Number of Courses: {courseCount}");
            Console.WriteLine($"Amount per Credit: {amountPerCredit} Rupees");
            Console.WriteLine($"Miscellaneous Fee: 15000 Rupees");


            foreach (var studentCourse in StudentCourses)
            {
                decimal amt = studentCourse.Course.Credits * amountPerCredit;
                Console.WriteLine($"{studentCourse.Course.Name} : {studentCourse.Course.Credits} * {amountPerCredit} = {amt} Rupees ");
                totalAmount += amt;
            }

            Console.WriteLine($"Total Amount:{totalAmount} Rupees");

            return totalAmount;
        }
    }
}
