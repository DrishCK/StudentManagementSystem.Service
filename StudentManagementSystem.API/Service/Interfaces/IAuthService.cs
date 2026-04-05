using StudentManagementSystem.API.DTOs;

namespace StudentManagementSystem.API.Service.Interfaces
{
    public interface IAuthService
    {
        Task<string> Login(LoginDto loginDto);
        Task<string> Register(RegisterDto registerDto);
    }
}
