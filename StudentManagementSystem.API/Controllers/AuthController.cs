using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.API.DTOs;
using StudentManagementSystem.API.Service;
using StudentManagementSystem.API.Service.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }

    // ✅ REGISTER
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var result = await _authService.Register(dto);

        return Ok(result);
    }

    // ✅ LOGIN
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await _authService.Login(dto);

        return Ok(result);
    }
}