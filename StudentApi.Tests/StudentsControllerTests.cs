using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SchoolManagementASPBackend.Controllers;
using SchoolManagementASPBackend.DTOs.Students;
using SchoolManagementASPBackend.Models;
using SchoolManagementASPBackend.Repositories;
using SchoolManagementASPBackend.Services;

namespace SchoolManagementASPBackend.Tests
{
    public class StudentsControllerTests
    {
        [Fact]
        public async Task GetStudentById_ExistingStudent_ReturnsOk()
        {
            //Arrange
            var student = new Student
            {
                Id = 15,
                Name = "John",
                Score = 85,
                Email = "john@example.com"
            };
            var response = new StudentResponse
            {
                Id = 15,
                Name = "John",
                Score = 85,
                Email = "john@example.com"
            };


            var repositoryMock = new Mock<IStudentRepository>();
            var loggerMock = new Mock<ILogger<IStudentService>>();

            var serviceMock = new Mock<IStudentService>();

            serviceMock
                   .Setup(s => s.GetStudentByIdAsync(15))
                   .ReturnsAsync(student);

            serviceMock
                .Setup(s => s.MapStudentResponse(student))
                .Returns(response);

            var controller = new StudentsController(serviceMock.Object);

            //Act
            var result = await controller.GetStudentById(15);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedStudent = Assert.IsType<StudentResponse>(okResult.Value);

            Assert.Equal(15, returnedStudent.Id);
            Assert.Equal("John", returnedStudent.Name);
            Assert.Equal(85, returnedStudent.Score);
            Assert.Equal("john@example.com", returnedStudent.Email);

            serviceMock.Verify(
                        s => s.GetStudentByIdAsync(15),
                        Times.Once);

            serviceMock.Verify(
                        s => s.MapStudentResponse(student),
                        Times.Once);


        }

        [Fact]
        public async Task GetStudentById_StudentNotFound_ReturnsNotFound()
        {
            //Arrange
            var repositoryMock = new Mock<IStudentRepository>();
            var loggerMock = new Mock<ILogger<IStudentService>>();

            var serviceMock = new Mock<IStudentService>();

            serviceMock
                   .Setup(s => s.GetStudentByIdAsync(999))
                   .ReturnsAsync((Student?)null);

            var controller = new StudentsController(serviceMock.Object);

            // Act
            var result = await controller.GetStudentById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);

            serviceMock.Verify(
    s => s.GetStudentByIdAsync(999),
    Times.Once);

            serviceMock.Verify(
    s => s.MapStudentResponse(It.IsAny<Student>()),
    Times.Never);

        }

        [Fact]
        public async Task CreateStudent_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var request = new CreateStudentRequest
            {
                Name = "Jane",
                Score = 90,
                Email = "jane@example.com"
            };

            var repositoryMock = new Mock<IStudentRepository>();
            var loggerMock = new Mock<ILogger<IStudentService>>();

            var serviceMock = new Mock<IStudentService>();

            serviceMock
                .Setup(s => s.CreateStudentAsync(It.IsAny<Student>()))
                .ReturnsAsync(15);

            var controller = new StudentsController(serviceMock.Object);

            //Act
            var result = await controller.CreateStudent(request);

            //Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
            var response = Assert.IsType<StudentResponse>(createdResult.Value);
            Assert.Equal(15, response.Id);
            Assert.Equal("Jane", response.Name);
            Assert.Equal(90, response.Score);
            Assert.Equal("jane@example.com", response.Email);

            Assert.Equal(
                nameof(StudentsController.GetStudentById),
                createdResult.ActionName);

            Assert.Equal(
                15,
                createdResult.RouteValues!["id"]);
        }
    }
}
