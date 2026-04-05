using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.API.Data;
using StudentManagementSystem.API.DTOs;
using StudentManagementSystem.API.Model;
using StudentManagementSystem.API.Service.Interfaces;

namespace StudentManagementSystem.API.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IMapper _mapper;

        public AuthService(AppDbContext context, JwtService jwtService, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<string> Login(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == loginDto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
                return ("Invalid credentials");

            var token = _jwtService.GenerateToken(user);

            return token;
        }

        public async Task<string> Register(RegisterDto registerDto)
        {
            if (await _context.Users.AnyAsync(x => x.Username == registerDto.Username))
                return ("User already exists");

            var user = new User
            {
                Username = registerDto.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return "User Created";
        }
    }
}
