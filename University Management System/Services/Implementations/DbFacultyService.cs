using Microsoft.EntityFrameworkCore;
using University_Management_System.Data;
using University_Management_System.Entities;
using University_Management_System.Exceptions;
using University_Management_System.Services.Interfaces;

namespace University_Management_System.Services.Implementations
{
    internal class DbFacultyService : IFacultyService
    {
        private readonly UniversityDbContext _dbContext;
        public DbFacultyService(UniversityDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddFacultyAsync(Faculty faculty)
        {
            try
            {
                _dbContext.Add(faculty);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during Adding new Faculty: {ex.Message}");
            }
        }

        public async Task<List<Faculty>> GetAllFacultiesAsync()
        {
            var faculties = await _dbContext.Faculties
            .Include(f => f.CoursesTaught)
            .ToListAsync() ?? throw new FacultyNotFoundException($"Faculties Not Found");
            return faculties;
        }

        public async Task<Faculty?> GetFacultyByIdAsync(int id)
        {
            Faculty faculty = await _dbContext?.Faculties.SingleOrDefaultAsync(f => f.Id == id) ?? throw new FacultyNotFoundException($"Faculty with Id:{id} Not Found:");
            return faculty;
        }

        public async Task RemoveFacultyAsync(int id)
        {
            Faculty faculty = await GetFacultyByIdAsync(id);
            try
            {
                _dbContext.Faculties.Remove(faculty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during Removing Faculty({id}): {ex.Message}");
            }

        }
    }
}
