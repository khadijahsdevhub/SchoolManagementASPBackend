using System.ComponentModel.DataAnnotations;

namespace SchoolManagementASPBackend.Models
{
    public class Student
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
        public int Score { get; set; }

        [MaxLength (100)]
        public string? Email { get; set; }

        public int DepartmentId { get; set; }

        public Department Department { get; set; } = null!;


    }
}
