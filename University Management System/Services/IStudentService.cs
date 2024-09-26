using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University_Management_System.Entities;

namespace University_Management_System.Services
{
    public interface IStudentService
    {
        Task AddStudentAsync(Student student);
        Task RemoveStudentAsync(int id);
        Task<List<Student>> GetAllStudentsAsync();
        Task<Student?> GetStudentByIdAsync(int id);
    }
}
