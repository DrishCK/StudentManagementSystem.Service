using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.API.DTOs;
using StudentManagementSystem.API.Model;
using StudentManagementSystem.API.Repositories.Interfaces;

namespace StudentManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;

        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
        }
        [Authorize]
        [HttpGet("GetAllStudents")]
        public async Task<IActionResult> GetAllStudents()
        { return Ok(await _studentRepository.GetAll()); }
        [Authorize]
        [HttpGet("GetStudentById")]
        public async Task<IActionResult> GetStudentById(int Id)
        {
            return Ok(await _studentRepository.GetById(Id));
        }
        [Authorize]
        [HttpPost("CreateStudent")]
        public async Task<IActionResult> CreateStudent(StudentDto student)
        {
            await _studentRepository.Create(student);
            return Ok("Student Added");
        }
        [Authorize]
        [HttpPut("UpdateStudent")]
        public async Task<IActionResult> UpdateStudent(StudentDto student)
        {
            await _studentRepository.Update(student);
            return Ok("Updated");
        }
        [Authorize]
        [HttpDelete("DeleteStudent")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            await _studentRepository.Delete(id);
            return Ok("Deleted");
        }
    }
}
