using Microsoft.EntityFrameworkCore;
using SchoolManagementASPBackend.DTOs.Students;
using SchoolManagementASPBackend.Models;
using SchoolManagementASPBackend.StudentApi.Data;

namespace SchoolManagementASPBackend.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        //private readonly string connectionString;
        //private readonly ILogger<StudentRepository> _logger;

        //public StudentRepository(string connectionString, ILogger<StudentRepository> logger)
        //{
        //    this.connectionString = connectionString;
        //    _logger = logger;
        //}

        private readonly SchoolDbContext _context;

        public StudentRepository(SchoolDbContext context)
        {
            _context = context;
        }

        //public async Task<int> CreateStudentAsync(Student student)
        //{
        //    using SqlConnection connection = new SqlConnection(connectionString);

        //    await connection.OpenAsync();

        //    string sql = """
        //    INSERT INTO Students (Name, Score, Email)
        //    VALUES (@Name, @Score, @Email);

        //    SELECT CAST(SCOPE_IDENTITY() AS INT);
        //    """;

        //    using SqlCommand command = new SqlCommand(sql, connection);

        //    command.Parameters.Add("@Name", SqlDbType.NVarChar, 100)
        //        .Value = student.Name;

        //    command.Parameters.Add("@Score", SqlDbType.Int)
        //        .Value = student.Score;

        //    command.Parameters.Add("@Email", SqlDbType.NVarChar, 100)
        //        .Value = (object?)student.Email ?? DBNull.Value;

        //    object? result = await command.ExecuteScalarAsync();

        //    return Convert.ToInt32(result);
        //}


        public async Task<int> CreateStudentAsync(Student student)
        {
            await _context.Students.AddAsync(student);

            await _context.SaveChangesAsync();

            return student.Id;
        }

        public async Task<Student?> GetStudentByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Students
                .FirstOrDefaultAsync(
                    s => s.Id == id,
                    cancellationToken);
        }

        public async Task<Student?> UpdateStudentAsync(Student student, CancellationToken cancellationToken)
        {
            var studentupdate = await _context.Students.FirstOrDefaultAsync(s => s.Id == student.Id, cancellationToken);
            if (studentupdate == null)
            {
                return null;
            }
            _context.Entry(studentupdate).CurrentValues.SetValues(student);

            await _context.SaveChangesAsync(cancellationToken);

            return studentupdate;

        }

        public async Task<(List<Student> Students, int TotalCount)> GetStudentsAsync(StudentQueryParameters studentQueryParameters, CancellationToken cancellationToken)
        {
            var query = _context.Students
                .AsNoTracking();

            if (studentQueryParameters.MinScore.HasValue)
            {
                query = query.Where(s =>
                    s.Score >= studentQueryParameters.MinScore.Value);
            }

            int totalCount = await query.CountAsync(cancellationToken);

            var students = await query
                .OrderBy(s => s.Id)
                .Skip((studentQueryParameters.Page - 1) * studentQueryParameters.PageSize)
                .Take(studentQueryParameters.PageSize)
                .ToListAsync(cancellationToken);

            return (students, totalCount);
        }

        //    public async Task<List<Student>> GetStudentsAsync(
        //int page,
        //int pageSize,
        //CancellationToken cancellationToken)
        //    {
        //        var students = await _context.Students
        //            .AsNoTracking()
        //            .Where(s => s.Score >= 70)
        //            .OrderByDescending(s => s.Score)
        //            .Select(s => new Student
        //            {
        //                Id = s.Id,
        //                Name = s.Name,
        //                Score = s.Score
        //            })
        //            .Skip((page - 1) * pageSize)
        //            .Take(pageSize)
        //            .ToListAsync(cancellationToken);

        //        return students;
        //    }


        //public async Task<Student?> GetStudentByIdAsync(int id)
        //{
        //    var builder = new SqlConnectionStringBuilder(connectionString);

        //    _logger.LogInformation("Student {StudentId} created successfully", id);

        //    using SqlConnection connection = new SqlConnection(connectionString);

        //    await connection.OpenAsync();

        //    string sql = """
        //SELECT Id, Name, Score, Email
        //FROM Students
        //WHERE Id = @Id;
        //""";

        //    using SqlCommand command = new SqlCommand(sql, connection);

        //    command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

        //    using SqlDataReader reader = await command.ExecuteReaderAsync();

        //    if (await reader.ReadAsync())
        //    {
        //        return new Student
        //        {
        //            Id = reader.GetInt32(reader.GetOrdinal("Id")),
        //            Name = reader.GetString(reader.GetOrdinal("Name")),
        //            Score = reader.GetInt32(reader.GetOrdinal("Score")),
        //            Email = reader.IsDBNull(reader.GetOrdinal("Email"))
        //                ? null
        //                : reader.GetString(reader.GetOrdinal("Email"))
        //        };
        //    }

        //    return null;
        //}
    }
}
