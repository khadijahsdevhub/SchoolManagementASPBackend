using Microsoft.AspNetCore.Mvc;
using SchoolManagementASPBackend.DTOs.Students;
using SchoolManagementASPBackend.Models;
using SchoolManagementASPBackend.Services;

namespace SchoolManagementASPBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService service;

        public StudentsController(IStudentService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(
       CreateStudentRequest request)
        {
            Student student = new Student
            {
                Name = request.Name,
                Score = request.Score,
                Email = request.Email
            };

            int studentId =
                await service.CreateStudentAsync(student);

            // Map the domain model and new ID to the StudentResponse DTO
            StudentResponse response = new StudentResponse
            {
                Id = studentId,
                Name = student.Name,
                Score = student.Score,
                Email = student.Email
            };

            // BEFORE
            // return Ok(studentId);

            // AFTER
            // return Created($"/api/students/{studentId}", studentId);

            // AFTER
            return CreatedAtAction(nameof(GetStudentById), new { id = studentId }, response);

        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            Student? student =
                await service.GetStudentByIdAsync(id);

            if (student is null)
                return NotFound();

            StudentResponse response = service.MapStudentResponse(student);

            return Ok(response);
        }


    }
}
