namespace SchoolManagementASPBackend.DTOs.Students
{
    public class StudentPaginationResponse
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        public List<StudentResponse> Students { get; set; }
            = new();
    }
}
