using SchoolManagementASPBackend.DTOs.Students;
using SchoolManagementASPBackend.Exceptions;
using SchoolManagementASPBackend.Models;
using SchoolManagementASPBackend.Repositories;

namespace SchoolManagementASPBackend.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository repository;
        private readonly ILogger<StudentService> logger;

        public StudentService(IStudentRepository repository, ILogger<StudentService> logger)
        {
            this.repository = repository;
            this.logger = logger;

        }

        public async Task<int> CreateStudentAsync(Student student)
        {

            if (string.IsNullOrWhiteSpace(student.Name))
                throw new ArgumentException("Name is required.");
            if (student.Score < 0 || student.Score > 100)
                throw new ArgumentException("Score must be between 0 and 100.");
            logger.LogInformation("Creating student with name {Name}", student.Name);
            int id = await repository.CreateStudentAsync(student);
            logger.LogInformation("Student {StudentId} created successfully", id);
            return id;
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            if (id <= 0)
                throw new InvalidStudentIdException("Student ID must be greater than zero.");

            return await repository.GetStudentByIdAsync(id);
        }

        public StudentResponse MapStudentResponse(Student student)
        {
            // Mapping Student to Student response
            return new StudentResponse
            {
                Id = student.Id,
                Name = student.Name,
                Score = student.Score,
                Email = student.Email
            };

        }

        public Student MapCreateStudentRequest(CreateStudentRequest request)
        {
            return new Student
            {
                Name = request.Name,
                Score = request.Score,
                Email = request.Email
            };
        }


    }
}
