using System.ComponentModel.DataAnnotations;

namespace SchoolManagementASPBackend.Models
{
    public class Department
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Student> Students { get; set; }
            = new List<Student>();
    }
}
