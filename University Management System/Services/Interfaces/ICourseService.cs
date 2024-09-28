using University_Management_System.Entities;

namespace University_Management_System.Services.Interfaces
{
    public interface ICourseService
    {
        Task AddCourseAsync(Course course);
        Task DeleteCourseAsync(int id);
        Task<List<Course>> GetAllCoursesAsync();
        Task<Course?> GetCourseByIdAsync(int id);
    }
}
