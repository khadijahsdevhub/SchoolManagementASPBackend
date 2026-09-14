using Microsoft.EntityFrameworkCore;
using SchoolManagementASPBackend.Models;

namespace SchoolManagementASPBackend.StudentApi.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(
            DbContextOptions<SchoolDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Department)
                .WithMany(d => d.Students)
                .HasForeignKey(s => s.DepartmentId);
        }
    }
}
