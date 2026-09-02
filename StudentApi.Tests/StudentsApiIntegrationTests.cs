using Microsoft.Data.SqlClient;
using SchoolManagementASPBackend.DTOs.Students;
using System.Net;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace SchoolManagementASPBackend.Tests
{

    public class StudentsApiIntegrationTests
    {
        private readonly ITestOutputHelper _output;
        public StudentsApiIntegrationTests(ITestOutputHelper output)
        {
            _output = output;
        }


        [Fact]
        public async Task GetStudentById_ExistingStudent_ReturnsOk()
        {
            // Arrange
            using var factory =
                new CustomWebApplicationFactory();

            using var client =
                factory.CreateClient();

            var studentId = 0;

            try {
                studentId =
                     await CreateTestStudentAsync();

                _output.WriteLine($"TEST STUDENT ID: {studentId}");
                // Act
                var response =
                    await client.GetAsync(
                        $"/api/students/{studentId}");

                // Assert
                Assert.Equal(
                    HttpStatusCode.OK,
                  response.StatusCode);

                var student =
                   await response.Content
                       .ReadFromJsonAsync<StudentResponse>();

                Assert.NotNull(student);
                _output.WriteLine($"Retrieved Student: {student?.Name}, Score: {student?.Score}, Email: {student?.Email}");
                Assert.Equal(studentId, student.Id);
                Assert.Equal(
                    "Integration Test Student",
                    student.Name);
                Assert.Equal(85, student.Score);
                Assert.Equal(
                    "integration@test.com",
                    student.Email);

            }
            
            finally
            {
                if (studentId > 0)
                {
                    await DeleteTestStudentAsync(studentId);
                }
            }
        }
    

private async Task<int> CreateTestStudentAsync()
        {
            var connectionString =
                "Server=(localdb)\\MSSQLLocalDB;Database=SchoolManagementTestDB;Trusted_Connection=True;TrustServerCertificate=True;";

            await using var connection =
                new SqlConnection(connectionString);

            await connection.OpenAsync();

            const string sql = """
        INSERT INTO Students (Name, Score, Email)
        OUTPUT INSERTED.Id
        VALUES ('Integration Test Student', 85, 'integration@test.com');
        """;

            await using var command =
                new SqlCommand(sql, connection);

            var result = await command.ExecuteScalarAsync();

            return Convert.ToInt32(result);
        }


        private async Task DeleteTestStudentAsync(int studentId)
        {
            var connectionString =
                "Server=(localdb)\\MSSQLLocalDB;Database=SchoolManagementTestDB;Trusted_Connection=True;TrustServerCertificate=True;";

            await using var connection =
                new SqlConnection(connectionString);

            await connection.OpenAsync();

            const string sql = """
        DELETE FROM Students
        WHERE Id = @Id;
        """;

            await using var command =
                new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Id", studentId);

            await command.ExecuteNonQueryAsync();
        }
    }
}
