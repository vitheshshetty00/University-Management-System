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


        public virtual decimal CalculateFees()
        {
            return 0;
        }
        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Email: {Email}, Date of Birth: {DateOfBirth.ToShortDateString()}, Enrollment Date: {EnrollmentDate.ToShortDateString()}, Address: {Address}";
        }
    }
}
