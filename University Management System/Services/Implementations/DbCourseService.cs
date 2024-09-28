using Microsoft.EntityFrameworkCore;
using University_Management_System.Data;
using University_Management_System.Entities;
using University_Management_System.Exceptions;
using University_Management_System.Services.Interfaces;

namespace University_Management_System.Services.Implementations
{
    public class DbCourseService : ICourseService
    {
        private readonly UniversityDbContext _dbContext;

        public DbCourseService(UniversityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddCourseAsync(Course course)
        {
            try
            {
                _dbContext.Courses.Add(course);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during Adding new Course: {ex.Message}");
            }
        }

        public async Task DeleteCourseAsync(int id)
        {
            Course course = await GetCourseByIdAsync(id);
            try
            {
                _dbContext.Courses.Remove(course);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during Removing Course({id}): {ex.Message}");
            }
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            List<Course> courses = await _dbContext.Courses.ToListAsync() ?? throw new CourseNotFoundException("Courses Not Found");
            return courses;
        }

        public async Task<Course?> GetCourseByIdAsync(int id)
        {
            Course course = await _dbContext.Courses.SingleOrDefaultAsync(c => c.Id == id) ?? throw new CourseNotFoundException($"Course with Id:{id} Not Found");
            return course;
        }
    }
}
