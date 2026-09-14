using SchoolManagementASPBackend.DTOs.Students;
using SchoolManagementASPBackend.Models;

namespace SchoolManagementASPBackend.Repositories
{
    public interface IStudentRepository
    {
        Task<int> CreateStudentAsync(Student student);

        Task<Student?> GetStudentByIdAsync(int id, CancellationToken cancellationToken);

        Task<Student?> UpdateStudentAsync(Student student, CancellationToken cancellationToken);

        Task<(List<Student> Students, int TotalCount)> GetStudentsAsync(StudentQueryParameters studentQueryParameters, CancellationToken cancellationToken);
    }
}
