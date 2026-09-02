using Microsoft.Extensions.Logging;
using Moq;
using SchoolManagementASPBackend.DTOs.Students;
using SchoolManagementASPBackend.Exceptions;
using SchoolManagementASPBackend.Models;
using SchoolManagementASPBackend.Repositories;
using SchoolManagementASPBackend.Services;

namespace SchoolManagementASPBackend.Tests
{
    public class StudentServiceTests
    {
        [Fact]
        public async Task GetStudentByIdAsync_ValidId_ReturnsStudent()
        {
            // Arrange
            var student = new Student
            {
                Id = 15,
                Name = "John",
                Score = 85,
                Email = "john@example.com"
            };
            var repositoryMock = new Mock<IStudentRepository>();

            repositoryMock
                .Setup(r => r.GetStudentByIdAsync(15))
                .ReturnsAsync(student);

            var loggerMock = new Mock<ILogger<StudentService>>();

            var service = new StudentService(
                repositoryMock.Object,
                loggerMock.Object);

            // Act
            var result = await service.GetStudentByIdAsync(15);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(15, result.Id);
            Assert.Equal("John", result.Name);
            Assert.Equal(85, result.Score);

            repositoryMock.Verify(
    r => r.GetStudentByIdAsync(15),
    Times.Once);

        }

        [Fact]
        public async Task GetStudentByIdAsync_InvalidId_ThrowsException()
        {
            // Arrange
            var repositoryMock = new Mock<IStudentRepository>();
            var loggerMock = new Mock<ILogger<StudentService>>();

            var service = new StudentService(
                repositoryMock.Object,
                loggerMock.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidStudentIdException>(() => service.GetStudentByIdAsync(-1));
            Assert.Equal("Student ID must be greater than zero.", exception.Message);
            repositoryMock.Verify(
                    r => r.GetStudentByIdAsync(-1),
                    Times.Never);
        }

        [Fact]
        public async Task CreateStudentAsync_ValidStudent_ReturnsCreatedStudentId()
        {
            // Arrange
            var student = new Student
            {
                Name = "John",
                Score = 85,
                Email = "john@example.com"
            };

            var repositoryMock = new Mock<IStudentRepository>();

            repositoryMock
                .Setup(r => r.CreateStudentAsync(student))
                .ReturnsAsync(15);

            var loggerMock = new Mock<ILogger<StudentService>>();
            var service = new StudentService(
                             repositoryMock.Object,
                                 loggerMock.Object);
            // Act
            var result = await service.CreateStudentAsync(student);

            // Assert
            Assert.Equal(15, result);

            repositoryMock.Verify(
                 r => r.CreateStudentAsync(student),
                Times.Once);
        }


        [Fact]
        public async Task CreateStudentAsync_InvalidName_ThrowsArgumentException()
        {
            // Arrange
            var student = new Student
            {
                Name = "",
                Score = 85,
                Email = "john@example.com"
            };

            var repositoryMock = new Mock<IStudentRepository>();
            var loggerMock = new Mock<ILogger<StudentService>>();

            var service = new StudentService(
                repositoryMock.Object,
                loggerMock.Object);


            // Act & Assert

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.CreateStudentAsync(student));
            Assert.Equal("Name is required.", exception.Message);
            repositoryMock.Verify(
                    r => r.CreateStudentAsync(student),
                    Times.Never);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        public async Task CreateStudentAsync_ValidScore_CreatesStudent(
    int score)
        {
            //Arrange
            var student = new Student
            {
                Name = "John",
                Score = score,
                Email = "john@example.com"
            };

            var repositoryMock = new Mock<IStudentRepository>();
            repositoryMock
                .Setup(r => r.CreateStudentAsync(student))
                .ReturnsAsync(15);

            var loggerMock = new Mock<ILogger<StudentService>>();
            var service = new StudentService(
                repositoryMock.Object,
                loggerMock.Object);

            //Act 
            var result = await service.CreateStudentAsync(student);

            //Assert
            Assert.Equal(15, result);

            repositoryMock.Verify(
                r => r.CreateStudentAsync(student),
                Times.Once);

        }

        [Theory]
        [InlineData(-1)]
        [InlineData(101)]
        public async Task CreateStudentAsync_InvalidScore_ThrowsArgumentException(int invalidScore)
        {
            // Arrange
            var student = new Student
            {
                Name = "John",
                Score = invalidScore,
                Email = "john@example.com"
            };

            var repositoryMock = new Mock<IStudentRepository>();
            var loggerMock = new Mock<ILogger<StudentService>>();

            var service = new StudentService(
                repositoryMock.Object,
                loggerMock.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.CreateStudentAsync(student));
            Assert.Equal("Score must be between 0 and 100.", exception.Message);
            repositoryMock.Verify(
                    r => r.CreateStudentAsync(student),
                    Times.Never);
        }

        [Fact]
        public void MapStudentResponse_ValidStudent_ReturnsStudentResponse()
        {
            // Arrange
            var student = new Student
            {
                Id = 15,
                Name = "John",
                Score = 85,
                Email = "john@example.com"
            };

            var repositoryMock = new Mock<IStudentRepository>();
            var loggerMock = new Mock<ILogger<StudentService>>();

            var service = new StudentService(
                repositoryMock.Object,
                loggerMock.Object);

            // Act
            var result = service.MapStudentResponse(student);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(student.Id, result.Id);
            Assert.Equal(student.Name, result.Name);
            Assert.Equal(student.Score, result.Score);
            Assert.Equal(student.Email, result.Email);
        }

        [Fact]
        public void MapCreateStudentRequest_ValidRequest_ReturnsStudent()
        {
            var student = new Student {
                Name = "Jane",
                Score = 90,
                Email = "jane@example.com"
            };

            var repositoryMock = new Mock<IStudentRepository>();
            var loggerMock = new Mock<ILogger<StudentService>>();

            var service = new StudentService(
                repositoryMock.Object,
                loggerMock.Object);

            // Act
            var result = service.MapCreateStudentRequest(new CreateStudentRequest
            {
                Name = student.Name,
                Score = student.Score,
                Email = student.Email
            });

            // Assert
            Assert.NotNull(result);
            Assert.Equal(student.Name, result.Name);
            Assert.Equal(student.Score, result.Score);
            Assert.Equal(student.Email, result.Email);

        }
    }
}
