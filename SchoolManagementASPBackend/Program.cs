using Microsoft.EntityFrameworkCore;
using SchoolManagementASPBackend.Exceptions;
using SchoolManagementASPBackend.Repositories;
using SchoolManagementASPBackend.Services;



var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the custom GlobalExceptionHandler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

string connectionString =
    builder.Configuration.GetConnectionString("SchoolDatabase")
    ?? throw new InvalidOperationException(
        "SchoolDatabase connection string is missing.");

builder.Services.AddScoped<IStudentRepository>(sp =>
    new StudentRepository(connectionString, sp.GetRequiredService<ILogger<StudentRepository>>()));
builder.Services.AddScoped<IStudentService, StudentService>();

var app = builder.Build();

// Enable exception handling middleware
app.UseExceptionHandler();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();


app.MapControllers();


app.Run();

public partial class Program { }