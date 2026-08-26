using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.Models;
using StudentManagement.Api.Services;
using StudentManagement.Api.Dtos;

namespace StudentManagement.Api.Controllers
{
    [Route("api/departments")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        // Injecting our IDepartmentService instead of using a static list
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // GET: api/departments
        [HttpGet]
        public IActionResult GetAllDepartments()
        {
            var departments = _departmentService.GetAllDepartments();
            return Ok(departments);
        }

        // GET: api/departments/{id}
        [HttpGet("{id}")]
        public IActionResult GetDepartmentById(int id)
        {
            var department = _departmentService.GetDepartmentById(id);
            if (department == null)
            {
                return NotFound($"Department with ID {id} not found.");
            }
            return Ok(department);
        }

        // POST: api/departments
        [HttpPost]
        public IActionResult AddDepartment([FromBody] Department newDepartment)
        {
            // Task 14: Department Validation
            if (string.IsNullOrWhiteSpace(newDepartment.Name))
            {
                return BadRequest("Department Name is required.");
            }

            var createdDepartment = _departmentService.AddDepartment(newDepartment);
            return Ok(createdDepartment);
        }

        // PUT: api/departments/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateDepartment(int id, [FromBody] Department updatedDepartment)
        {
            // Task 14: Department Validation
            if (string.IsNullOrWhiteSpace(updatedDepartment.Name))
            {
                return BadRequest("Department Name is required.");
            }

            var existingDepartment = _departmentService.UpdateDepartment(id, updatedDepartment);
            if (existingDepartment == null)
            {
                return NotFound($"Cannot edit: Department with ID {id} not found.");
            }

            return Ok(existingDepartment);
        }

        // DELETE: api/departments/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteDepartment(int id)
        {
            bool isDeleted = _departmentService.DeleteDepartment(id);

            if (!isDeleted)
            {
                return NotFound($"Cannot delete: Department with ID {id} not found.");
            }

            return Ok($"Department with ID {id} has been deleted successfully.");
        }

        // Task 12: Department Statistics (GET /api/departments/statistics)
        [HttpGet("statistics")]
        public IActionResult GetDepartmentStatistics()
        {
            // Fully delegated to the database service!
            var stats = _departmentService.GetDepartmentStatistics();
            return Ok(stats);
        }

        // Task 13: Highest and Lowest Department (GET /api/departments/highest-lowest)
        [HttpGet("highest-lowest")]
        public IActionResult GetHighestAndLowestDepartment()
        {
            // Fully delegated to the database service!
            var result = _departmentService.GetHighestAndLowestDepartments();
            return Ok(result);
        }
    }
}