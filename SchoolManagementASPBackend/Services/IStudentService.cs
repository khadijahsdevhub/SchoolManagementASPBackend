using SchoolManagementASPBackend.Models;

namespace SchoolManagementASPBackend.Services
{
    public interface IStudentService
    {
        Task<int> CreateStudent(Student student);

        Task<Student> GetStudentById(int id);

        Task<List<Student>> GetAllStudents();

        Task UpdateStudent(Student student);
        Task DeleteStudent(int id);
    }
}
