
using StudentManagementSystem.API.DTOs;

namespace StudentManagementSystem.API.Repositories.Interfaces
{
    public interface IStudentRepository
    {
        Task<IList<StudentDto>> GetAll();
        Task<StudentDto> GetById(int id);
        Task Create(StudentDto student);
        Task Update(StudentDto student);
        Task Delete(int id);
    }
}
