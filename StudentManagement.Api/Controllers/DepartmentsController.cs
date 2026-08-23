using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.Models;


namespace StudentManagement.Api.Controllers
{
    [Route("api/departments")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        //We make this public static so our StudentService can acceess it later for validation
        public static List<Department> Departments = new List<Department>
        {
            new Department { Id = 1, Name = "IT" },
            new Department { Id = 2, Name = "HR" },
            new Department { Id = 3, Name = "Finance" },
            new Department { Id = 4, Name = "Sales" },
        };
        // GET: api/departments
        [HttpGet]
        public IActionResult GetAllDepartments()
        {
            return Ok(Departments);
        }

        // GET: api/departments/{id}
        [HttpGet("{id}")]
        public IActionResult GetDepartmentById(int id)
        {
            var department = Departments.FirstOrDefault(d => d.Id == id);
            if (department == null)
            {
                return NotFound($"Department with ID {id} not found.");
            }
            return Ok(department);
        }

        //Post: api/departments
        [HttpPost]
        public IActionResult AddDepartment([FromBody] Department newDepartment)
        {
            // Generate a new IF (finding max ID and addding 1)
            int newID = Departments.Max(d => d.Id) + 1;

            newDepartment.Id = newID;
            Departments.Add(newDepartment);

            return Ok(newDepartment);
        }

        // PUT: api/departments/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateDepartment(int id, [FromBody] Department updatedDepartment)
        {
            var existingDepartment = Departments.FirstOrDefault(d => d.Id == id);
            if (existingDepartment == null)
            {
                return NotFound($"Cannot edit: Department with ID {id} not found.");
            }

            existingDepartment.Name = updatedDepartment.Name;
            return Ok(existingDepartment);
        }

        // DELETE: api/departments/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteDepartment(int id)
        {
            var department = Departments.FirstOrDefault(d => d.Id == id);

            if (department == null)
            {
                return NotFound($"Cannot delete: Department with ID {id} not found.");
            }
            Departments.Remove(department);
            return Ok($"Department with ID {id} has been deleted.");
        }
    }
}
