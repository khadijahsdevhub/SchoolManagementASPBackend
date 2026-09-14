using System.ComponentModel.DataAnnotations;

namespace SchoolManagementASPBackend.DTOs.Students
{
    public class CreateStudentRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
        [Range(0, 100)]
        public int Score { get; set; }
        [EmailAddress]
        public string? Email { get; set; }

        public int DepartmentId { get; set; }
    }
}
