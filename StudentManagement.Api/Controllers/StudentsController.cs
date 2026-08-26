using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.Models;
using StudentManagement.Api.Dtos;
using StudentManagement.Api.Services;

namespace StudentManagement.Api.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IDepartmentService _departmentService; // Added this to validate departments from the DB

        // Inject both services so we can manage students and validate departments
        public StudentsController(IStudentService studentService, IDepartmentService departmentService)
        {
            _studentService = studentService;
            _departmentService = departmentService;
        }

        [HttpGet("welcome")]
        public IActionResult Welcome()
        {
            return Ok("Welcome to the Student Management API!");
        }

        [HttpGet]
        public IActionResult GetAllStudents()
        {
            var students = _studentService.GetAllStudents();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            var student = _studentService.GetStudentById(id);

            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            return Ok(student);
        }

        // Task 10: Search Students Or Departments (GET /api/students/search?text={text})
        [HttpGet("search")]
        public IActionResult SearchStudents([FromQuery] string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return BadRequest("Please provide text to search for.");
            }

            var matchingStudents = _studentService.SearchStudents(text);
            return Ok(matchingStudents);
        }

        // Task 11: Filter Students by Age (GET /api/students/filter-by-age)
        [HttpGet("filter-by-age")]
        public IActionResult FilterByAge()
        {
            var filteredStudents = _studentService.FilterByAge();
            return Ok(filteredStudents);
        }

        [HttpPost]
        public IActionResult AddStudent([FromBody] CreateStudentDto dto)
        {
            // Task 14: Add Basic Validation
            if (string.IsNullOrWhiteSpace(dto.Name)) return BadRequest("Name is required.");
            if (dto.Age < 18 || dto.Age > 60) return BadRequest("Age must be between 18 and 60.");

            // Validate department exists using the database service
            var department = _departmentService.GetDepartmentById(dto.DepartmentId);
            if (department == null)
            {
                return BadRequest($"Validation failed: Department with ID {dto.DepartmentId} does not exist.");
            }

            var newStudent = _studentService.AddStudent(dto);
            return Ok(newStudent);
        }

        [HttpPut("{id}")]
        public IActionResult EditStudent(int id, [FromBody] UpdateStudentDto dto)
        {
            // Task 14: Add Basic Validation
            if (string.IsNullOrWhiteSpace(dto.Name)) return BadRequest("Name is required.");
            if (dto.Age < 18 || dto.Age > 60) return BadRequest("Age must be between 18 and 60.");

            // Validate department exists using the database service
            var department = _departmentService.GetDepartmentById(dto.DepartmentId);
            if (department == null)
            {
                return BadRequest($"Validation failed: Department with ID {dto.DepartmentId} does not exist.");
            }

            var updatedStudent = _studentService.UpdateStudent(id, dto);

            if (updatedStudent == null)
            {
                return NotFound($"Cannot edit: Student with ID {id} not found.");
            }

            return Ok(updatedStudent);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            bool isDeleted = _studentService.DeleteStudent(id);

            if (!isDeleted)
            {
                return NotFound($"Cannot delete: Student with ID {id} not found.");
            }

            return Ok($"Student with ID {id} has been deleted successfully.");
        }
    }
}