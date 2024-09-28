
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Data;
using University_Management_System.Entities;
using University_Management_System.Exceptions;
using University_Management_System.Services.Interfaces;

namespace University_Management_System.Services.Implementations
{
    public class DbStudentService : IStudentService
    {
        private readonly UniversityDbContext? _dbContext;

        public DbStudentService(UniversityDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException("Databse Context is not initialized.");
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

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            List<Student> students = await _dbContext?.Students?.ToListAsync()
                ?? throw new StudentNotFoundException("Students Not Found.");

            return students;
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            Student? student = await (_dbContext?.Students?.Include(s => s.StudentCourses)
                .ThenInclude(sc => sc.Course)
                .FirstOrDefaultAsync(s => s.Id == id)) ?? throw new StudentNotFoundException($"Student with Id {id} not found.");
            return student;
        }

        public async Task RemoveStudentAsync(int id)
        {
            var student = await GetStudentByIdAsync(id);

            _dbContext?.Students.Remove(student);
            await _dbContext.SaveChangesAsync();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{student.Name}({student.Id}) Has been removed Succesfully");
            Console.ResetColor();


        }

        public async Task UpdateStudentAsync(Student student)
        {
            try
            {
                var existingStudent = await GetStudentByIdAsync(student.Id);
                if (existingStudent == null)
                {
                    throw new StudentNotFoundException($"Student with Id {student.Id} not found.");
                }


                _dbContext.Entry(existingStudent).CurrentValues.SetValues(student);


                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the student: {ex.Message}");
                throw;
            }

        }
        public async Task ADOUpdateStudentAsync(Student student)
        {
            string connectionString = _dbContext.Database.GetDbConnection().ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
            UPDATE Students
            SET Name = @Name, Email = @Email, DateOfBirth = @DateOfBirth, 
                EnrollmentDate = @EnrollmentDate, Fees = @Fees, PaymentStatus = @PaymentStatus
            WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", student.Id);
                    command.Parameters.AddWithValue("@Name", student.Name ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Email", student.Email ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
                    command.Parameters.AddWithValue("@EnrollmentDate", student.EnrollmentDate);
                    command.Parameters.AddWithValue("@Fees", student.Fees ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PaymentStatus", student.PaymentStatus);

                    try
                    {
                        await connection.OpenAsync();
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected == 0)
                        {
                            throw new StudentNotFoundException($"Student with Id {student.Id} not found.");
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"SQL Exception: {ex.Message}");
                        throw;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred while updating the student: {ex.Message}");
                        throw;
                    }
                }
            }
        }

    }
}
