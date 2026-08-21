namespace SchoolManagementASPBackend.DTOs.Students
{
    public class StudentResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Score { get; set; }
        public string? Email { get; set; }
    }
}
