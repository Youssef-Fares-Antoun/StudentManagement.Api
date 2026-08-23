using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.Models; // Added this to access new Models
using StudentManagement.Api.Dtos; // Added this to access new Dtos
using StudentManagement.Api.Services; // Added this to access new Services

namespace StudentManagement.Api.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        // 1. Declare a private, read-only field to hold our service interface.
        // Using an interface (IStudentService) instead of the concrete class (StudentService) 
        // makes our application loosely coupled and easier to test.
        private readonly IStudentService _studentService;

        // 2. Constructor Injection: When .NET creates this controller to handle a request, 
        // it automatically creates a StudentService (because we registered it in Program.cs) 
        // and passes it in here.
        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        //Task 2: Welcome Endpoint (GET /api/students/welcome)
        [HttpGet("welcome")]
        public IActionResult Welcome()
        {
            return Ok("Welcome to the Student Management API!");
        }

        //Task 3: Get All Students (GET /api/students)
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            // The controller no longer handles data logic. It simply asks the service 
            // for the data and wraps the result in an HTTP 200 OK response.
            var students = _studentService.GetAllStudents();
            return Ok(students);
        }

        //Task 4: Get SStudent By Id (GET /api/students/{id})
        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            // Ask the service to find the student
            var student = _studentService.GetStudentById(id);

            // If the service returns null, the controller knows to return a 404 Not Found
            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            // Otherwise, return the student with a 200 OK
            return Ok(student);
        }

        // Task 5: Search Students By Name (GET /api/students/search?name={name})
        [HttpGet("search")]
        public IActionResult SearchStudents([FromQuery] string name)
        {
            //If the user doesn't provide a name, return a BadRequest
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Please provide a name to search for.");
            }

            // Delegate the searching logic entirely to the service
            var matchingStudents = _studentService.SearchStudents(name);
            return Ok(matchingStudents);
        }

        // Task 6: Filter Students by Age (GET /api/students/filter-by-age)
        [HttpGet("filter-by-age")]
        public IActionResult FilterByAge()
        {
            // Delegate the filtering and sorting logic to the service
            var filteredStudents = _studentService.FilterByAge();
            return Ok(filteredStudents);
        }

        // Task 7: Add New Stuent In Memory (POST /api/students)
        [HttpPost]
        public IActionResult AddStudent([FromBody] CreateStudentDto dto)
        {
            // 1. Check if the provided DepartmentId exists in our shared Departments list
            bool isValidDepartment = DepartmentsController.Departments.Any(d => d.Id == dto.DepartmentId);

            // 2. If it doesn't exist, return a 400 Bad Request immediately
            if (!isValidDepartment)
            {
                return BadRequest($"Validation failed: Department with ID {dto.DepartmentId} does not exist.");
            }

            // 3. If valid, proceed with adding the student via the service
            var newStudent = _studentService.AddStudent(dto);
            return Ok(newStudent);
        }

        //Task 8: Edit Student In Memory (PUT /api/students/{id})
        [HttpPut("{id}")]
        public IActionResult EditStudent(int id, [FromBody] UpdateStudentDto dto)
        {
            // 1. Validate the new DepartmentId
            bool isValidDepartment = DepartmentsController.Departments.Any(d => d.Id == dto.DepartmentId);

            if (!isValidDepartment)
            {
                return BadRequest($"Validation failed: Department with ID {dto.DepartmentId} does not exist.");
            }

            // 2. If valid, proceed with the update
            var updatedStudent = _studentService.UpdateStudent(id, dto);

            if (updatedStudent == null)
            {
                return NotFound($"Cannot edit: Student with ID {id} not found.");
            }

            return Ok(updatedStudent);
        }

        // Task 9:Delete Student In Memory (DELETE /api/students/{id})
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            // The service attempts to delete the student and returns a true/false success flag.
            bool isDeleted = _studentService.DeleteStudent(id);

            if (!isDeleted)
            {
                return NotFound($"Cannot delete: Student with ID {id} not found.");
            }

            return Ok($"Student with ID {id} has been deleted successfully.");
        }
    }
}