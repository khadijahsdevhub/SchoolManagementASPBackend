using Microsoft.Data.SqlClient;
using SchoolManagementASPBackend.Models;
using System.Data;

namespace SchoolManagementASPBackend.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly string connectionString;
        private readonly ILogger<StudentRepository> _logger;

        public StudentRepository(string connectionString, ILogger<StudentRepository> logger)
        {
            this.connectionString = connectionString;
            _logger = logger;
        }

        public async Task<int> CreateStudentAsync(Student student)
        {
            using SqlConnection connection = new SqlConnection(connectionString);

            await connection.OpenAsync();

            string sql = """
            INSERT INTO Students (Name, Score, Email)
            VALUES (@Name, @Score, @Email);

            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

            using SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.Add("@Name", SqlDbType.NVarChar, 100)
                .Value = student.Name;

            command.Parameters.Add("@Score", SqlDbType.Int)
                .Value = student.Score;

            command.Parameters.Add("@Email", SqlDbType.NVarChar, 100)
                .Value = (object?)student.Email ?? DBNull.Value;

            object? result = await command.ExecuteScalarAsync();

            return Convert.ToInt32(result);
        }



        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);

            _logger.LogInformation("Student {StudentId} created successfully", id);

            using SqlConnection connection = new SqlConnection(connectionString);

            await connection.OpenAsync();

            string sql = """
        SELECT Id, Name, Score, Email
        FROM Students
        WHERE Id = @Id;
        """;

            using SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Student
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Score = reader.GetInt32(reader.GetOrdinal("Score")),
                    Email = reader.IsDBNull(reader.GetOrdinal("Email"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("Email"))
                };
            }

            return null;
        }
    }
}
