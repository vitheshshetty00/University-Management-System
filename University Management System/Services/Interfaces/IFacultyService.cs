using University_Management_System.Entities;

namespace University_Management_System.Services.Interfaces
{
    public interface IFacultyService
    {
        Task AddFacultyAsync(Faculty faculty);
        Task RemoveFacultyAsync(int id);
        Task<List<Faculty>> GetAllFacultiesAsync();
        Task<Faculty?> GetFacultyByIdAsync(int id);
    }
}
