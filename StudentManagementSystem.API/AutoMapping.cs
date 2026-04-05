using AutoMapper;
using StudentManagementSystem.API.DTOs;
using StudentManagementSystem.API.Model;

namespace StudentManagementSystem.API
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Student, StudentDto>().ReverseMap();
        }
    }
}
