using SchoolManagementASPBackend.DTOs.Students;
using SchoolManagementASPBackend.Models;

namespace SchoolManagementASPBackend.Services
{
    public interface IStudentService
    {
        Task<int> CreateStudentAsync(Student student);

        Task<Student?> GetStudentByIdAsync(int id);

        StudentResponse MapStudentResponse(Student student);

        Student MapCreateStudentRequest(CreateStudentRequest request);


        //  Task<List<Student>> GetAllStudents();

        // Task UpdateStudent(Student student);
        //  Task DeleteStudent(int id);
    }
}
