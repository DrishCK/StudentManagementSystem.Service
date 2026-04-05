
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StudentManagementSystem.API.Data;
using StudentManagementSystem.API.Middleware;
using StudentManagementSystem.API.Repositories;
using StudentManagementSystem.API.Repositories.Interfaces;
using StudentManagementSystem.API.Service;
using StudentManagementSystem.API.Service.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add Controllers
builder.Services.AddControllers();

// ✅ Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Student API",
        Version = "v1"
    });

    // 🔐 Add JWT Security Definition
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Enter 'Bearer' followed by space and token\nExample: Bearer eyJhbGciOiJIUzI1NiIs...",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.OperationFilter<AuthorizeCheckOperationFilter>();
});

// ✅ DbContext (SQL Server)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnectionString")));

// ✅ AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// ✅ Dependency Injection (Repository)
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<JwtService>();

// ✅ JWT Authentication
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// ✅ Global Exception Middleware
//app.UseMiddleware<ExceptionMiddleware>();

// ✅ Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();
// ✅ IMPORTANT: Authentication BEFORE Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();