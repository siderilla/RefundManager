using Microsoft.EntityFrameworkCore;
using RefundManager.Data;
using RefundManager.Interfaces;
using RefundManager.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var cs = builder.Configuration.GetConnectionString("Postgres");

builder.Services.AddDbContext<RefundManagerContext>(opt =>
opt.UseNpgsql(cs));

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
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

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
