using SchoolManagementASPBackend.Models;

namespace SchoolManagementASPBackend.Repositories
{
    public interface IStudentRepository
    {
        Task<int> CreateStudentAsync(Student student);

        Task<Student?> GetStudentByIdAsync(int id);
    }
}
