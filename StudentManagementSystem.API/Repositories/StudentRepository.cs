using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.API.Data;
using StudentManagementSystem.API.DTOs;
using StudentManagementSystem.API.Model;
using StudentManagementSystem.API.Repositories.Interfaces;

namespace StudentManagementSystem.API.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public StudentRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IList<StudentDto>> GetAll()
        {
            var data = await _context.Students.ToListAsync();

            var result = _mapper.Map<IList<StudentDto>>(data);

            return result;
        }
        public async Task<StudentDto> GetById(int id)
        {
            var data = await _context.Students.FindAsync(id);
            var result = _mapper.Map<StudentDto>(data);
            return result;
        }

        public async Task Create(StudentDto student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student), "Student data is required");

            var exists = await _context.Students
                .AnyAsync(s => s.Email == student.Email);

            if (exists)
                throw new Exception("Student with this email already exists");

            var createData = _mapper.Map<Student>(student);
            createData.CreatedDate = DateTime.UtcNow;

            await _context.Students.AddAsync(createData);
            await _context.SaveChangesAsync();
        }

        public async Task Update(StudentDto student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student), "Student data is required");

            if (student.Id == null)
                throw new Exception("Student ID is required");

            var updateData = await _context.Students.FindAsync(student.Id);

            if (updateData == null)
                throw new Exception("Student not found");

            var exists = await _context.Students
                .AnyAsync(s => s.Email == student.Email && s.Id != student.Id);

            if (exists)
                throw new Exception("Another student with this email already exists");

            updateData.Name = student.Name;
            updateData.Email = student.Email;
            updateData.Age = student.Age;
            updateData.Course = student.Course;

            updateData.UpdatedDate = DateTime.UtcNow;

            _context.Students.Update(updateData);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var dataToDelete = await _context.Students.FindAsync(id); ;
            _context.Students.Remove(dataToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
