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
        //1. Injecting the IStudentSeervice so we can grab student data
        private readonly IStudentService _studentService;
        //2. Constructor Injection
        public DepartmentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public static List<Department> Departments = new List<Department>
        {
            new Department { Id = 1, Name = "Computer Science" },
            new Department { Id = 2, Name = "Mathematics" },
            new Department { Id = 3, Name = "Physics" }
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
        //Task 9: Department Statistics (Get /api/departments/statistics)
        [HttpGet("statistics")]
        public IActionResult GetDepartmentStatistics()
        {
            //1.Get all students from the service
            var allStudents = _studentService.GetAllStudents();

            //2. Looping through our departments and calculating the stats
            var statistics = Departments.Select(d =>
            {
                //Finding all students that belong to this specific depatment
                var deptStudents = allStudents.Where(s => s.DepartmentName == d.Name).ToList();

                return new DepartmentStatisticsDto
                {
                    DepartmentName = d.Name,
                    StudentCount = deptStudents.Count,
                    //cheecking .Any() First to avoid dividing by zero if a department has no students
                    AverageAge = deptStudents.Any() ? Math.Round(deptStudents.Average(s => s.Age), 2) : 0,
                    OldestAge = deptStudents.Any() ? deptStudents.Max(s => s.Age) : (int?)null,
                    YoungestAge = deptStudents.Any() ? deptStudents.Min(s => s.Age) : (int?)null
                };
            }).ToList();

            return Ok(statistics);
        }

        // Task 10: Highest and Lowest Department (GET /api/departments/highest-lowest)
        [HttpGet("highest-lowest")]
        public IActionResult GetHighestAndLowestDepartment()
        {
            var allStudents = _studentService.GetAllStudents();

            // 1. Get a simple list of each department and its student count
            var departmentCounts = Departments.Select(d => new
            {
                DepartmentName = d.Name,
                StudentCount = allStudents.Count(s => s.DepartmentName == d.Name)
            }).ToList();

            // 2. Sort to find the highest and lowest
            var highest = departmentCounts.OrderByDescending(d => d.StudentCount).FirstOrDefault();
            var lowest = departmentCounts.OrderBy(d => d.StudentCount).FirstOrDefault();

            // 3. Return an anonymous object containing both
            return Ok(new
            {
                Highest = highest,
                Lowest = lowest
            });
        }
    }
}
