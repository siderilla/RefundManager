using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RefundManager.Data;
using RefundManager.Interfaces;
using RefundManager.Models;
using RefundManager.Services;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var cs = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<RefundManagerContext>(opt =>
opt.UseNpgsql(cs));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.WithOrigins("http://localhost:4200")
    .AllowAnyHeader()
    .AllowAnyMethod()
));
builder.Services.AddScoped<IOutlayService, OutlayService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IRefundService, RefundService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<PasswordHasher<Employee>>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Demo-only: make sure every seed employee has a usable password, so the
    // README's shared demo credential ("Demo2026!") always works after a fresh db restore.
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<RefundManagerContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<PasswordHasher<Employee>>();

        var employeesWithoutPassword = context.Employees.Where(e => e.PasswordHash == null).ToList();
        foreach (var employee in employeesWithoutPassword)
        {
            employee.PasswordHash = hasher.HashPassword(employee, "Demo2026!");
        }

        if (employeesWithoutPassword.Count > 0)
        {
            context.SaveChanges();
        }
    }
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
