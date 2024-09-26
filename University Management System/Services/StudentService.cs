
using University_Management_System.Data;
using University_Management_System.Entities;
using University_Management_System.Exceptions;

namespace University_Management_System.Services
{
    public class StudentService : IStudentService
    {
        private readonly UniversityDbContext? _dbContext;

        public StudentService(UniversityDbContext dbContext)
        {
            _dbContext = dbContext?? throw new ArgumentNullException("Databse Context is not initialized.");
        }
        public async Task AddStudentAsync(Student student)
        {
            try
            {
                _dbContext.Students.Add(student);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding the student: {ex.Message}");
                throw;
            }
        }

        public Task<List<Student>> GetAllStudentsAsync()
        {
            List<Student> students = _dbContext.Students.ToList();
            return Task.FromResult(students);
        }

        public Task<Student?> GetStudentByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveStudentAsync(int id)
        {
            var student = await _dbContext.Students.FindAsync(id);
            //Console.WriteLine(student.ToString());
            if (student == null)
            {
                throw new StudentNotFoundException($"Student with Id {id} not found.");
            }

            _dbContext.Students.Remove(student);
            await _dbContext.SaveChangesAsync();
        }
    }
}
