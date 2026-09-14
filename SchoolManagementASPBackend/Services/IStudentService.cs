using SchoolManagementASPBackend.DTOs.Students;
using SchoolManagementASPBackend.Models;

namespace SchoolManagementASPBackend.Services
{
    public interface IStudentService
    {
        Task<int> CreateStudentAsync(Student student);

        Task<Student?> GetStudentByIdAsync(int id);

        Task<Student?> UpdateStudentAsync(Student updateStudentRequest, CancellationToken cancellationToken);

        Task<StudentPaginationResponse> GetStudentsAsync(int page, int pageSize, CancellationToken cancellationToken);

        StudentResponse MapStudentResponse(Student student);

        Student MapCreateStudentRequest(CreateStudentRequest request);


       
    }
}
