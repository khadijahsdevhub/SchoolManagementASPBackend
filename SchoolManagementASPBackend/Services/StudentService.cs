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

            // Ensure a valid department is assigned; default to General (Id = 1)
            if (student.DepartmentId == 0)
                student.DepartmentId = 1;

            logger.LogInformation("Creating student with name {Name}", student.Name);
            int id = await repository.CreateStudentAsync(student);
            logger.LogInformation("Student {StudentId} created successfully", id);
            return id;
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            if (id <= 0)
                throw new InvalidStudentIdException("Student ID must be greater than zero.");

            return await repository.GetStudentByIdAsync(id, CancellationToken.None);
        }

        public async Task<Student?> UpdateStudentAsync(Student updateStudentRequest, CancellationToken cancellationToken)
        {
            if (updateStudentRequest.Id <= 0)
                throw new InvalidStudentIdException(
                    "Student ID must be greater than zero.");

            if (string.IsNullOrWhiteSpace(updateStudentRequest.Name))
                throw new ArgumentException("Name is required.");
            if (updateStudentRequest.Score < 0 || updateStudentRequest.Score > 100)
                throw new ArgumentException("Score must be between 0 and 100.");

            // Ensure a valid department is assigned; default to General (Id = 1)
            if (updateStudentRequest.DepartmentId == 0)
                updateStudentRequest.DepartmentId = 1;

            var updatedStudent = await repository.UpdateStudentAsync(updateStudentRequest, cancellationToken);

            if (updatedStudent == null)
            {

                throw new StudentNotFoundException($"Student with ID {updateStudentRequest.Id} not found.");
            }

            return updatedStudent;
        }

        public async Task<StudentPaginationResponse> GetStudentsAsync(StudentQueryParameters studentQueryParameters, CancellationToken cancellationToken)
        {
            if (studentQueryParameters.Page <= 0)
                throw new ArgumentException("Page number must be greater than zero.");
            if (studentQueryParameters.PageSize <= 0 || studentQueryParameters.PageSize > 100)
                throw new ArgumentException("Page size must be greater than zero and less than or equal to 100.");
            var (students, totalCount) = await repository.GetStudentsAsync(studentQueryParameters, cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalCount / studentQueryParameters.PageSize);
            return new StudentPaginationResponse
            {
                Page = studentQueryParameters.Page,
                PageSize = studentQueryParameters.PageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Students = students.Select(MapStudentResponse).ToList()
            };
        }

        public StudentResponse MapStudentResponse(Student student)
        {
            // Mapping Student to Student response
            return new StudentResponse
            {
                Id = student.Id,
                Name = student.Name,
                Score = student.Score,
                Email = student.Email,
                DepartmentId = student.DepartmentId

            };

        }

        public Student MapCreateStudentRequest(CreateStudentRequest request)
        {
            return new Student
            {
                Name = request.Name,
                Score = request.Score,
                Email = request.Email,
                DepartmentId = request.DepartmentId
            };
        }


    }
}
