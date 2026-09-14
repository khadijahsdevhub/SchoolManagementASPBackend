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
                Email = request.Email,
                DepartmentId = request.DepartmentId
            };

            int studentId =
                await service.CreateStudentAsync(student);

            // Map the domain model and new ID to the StudentResponse DTO
            StudentResponse response = new StudentResponse
            {
                Id = studentId,
                Name = student.Name,
                Score = student.Score,
                Email = student.Email,
                DepartmentId = student.DepartmentId
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

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateStudent(int id, CreateStudentRequest request)
        {

            Student studentUpdate = new Student
            {
                Email = request.Email,
                DepartmentId = request.DepartmentId,
                Id = id,
                Name = request.Name,
                Score = request.Score,
            };

            var updatedStudent = await service.UpdateStudentAsync(studentUpdate, HttpContext.RequestAborted);
            StudentResponse response = service.MapStudentResponse(updatedStudent);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents([FromQuery] StudentQueryParameters parameters)
        {
            var studentPaginationResponse = await service.GetStudentsAsync(parameters.Page, parameters.PageSize, HttpContext.RequestAborted);
            return Ok(studentPaginationResponse);
        }

    }
}
