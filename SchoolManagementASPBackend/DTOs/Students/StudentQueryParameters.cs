namespace SchoolManagementASPBackend.DTOs.Students
{
    public class StudentQueryParameters
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int? MinScore { get; set; }
    }
}
