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
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? new[] { "http://localhost:4200" };

builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.WithOrigins(allowedOrigins)
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

// Apply pending EF Core migrations on startup. There's no shell access on the
// deploy target to run `dotnet ef database update` by hand, so the app has to
// bring the schema up to date itself on boot.
using (var migrationScope = app.Services.CreateScope())
{
    migrationScope.ServiceProvider.GetRequiredService<RefundManagerContext>().Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Off by default so a local `dotnet run` never touches your own test data.
// Set SeedDemoData=true (as an env var) only on the public deploy, where it
// keeps the demo employees/categories/password seeded and regenerates the
// outlay/refund rows on every restart so the dashboard's calendar always has
// something in the current month.
if (builder.Configuration.GetValue<bool>("SeedDemoData"))
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<RefundManagerContext>();
    var hasher = scope.ServiceProvider.GetRequiredService<PasswordHasher<Employee>>();
    DemoDataSeeder.Seed(context, hasher);
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
