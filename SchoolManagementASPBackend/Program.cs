var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();


string connectionString =
    builder.Configuration.GetConnectionString("SchoolDatabase")
    ?? throw new InvalidOperationException(
        "SchoolDatabase connection string is missing.");

builder.Services.AddScoped<IStudentRepository>(
    _ => new StudentRepository(connectionString));