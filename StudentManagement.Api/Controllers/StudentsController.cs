using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.Models; // Added this to access new Models

namespace StudentManagement.Api.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        //Task 3: Create a static listt of students

        private static List<Student> students = new List<Student>
        {
            new Student { Id = 1, Name = "Ahmed", Age = 20, DepartmentId = 1 },
            new Student { Id = 2, Name = "Sara", Age = 22, DepartmentId = 2 },
            new Student { Id = 3, Name = "Omar", Age = 19, DepartmentId = 3 }
        };

        //Task 3: Create a static list of departments
        private static List<Department> departments = new List<Department>
        {
            new Department { ID = 1, Name = "IT" },
            new Department { ID = 2, Name = "HR" },
            new Department { ID = 3, Name = "Finance" },
            new Department {ID = 4, Name = "Sales" }
        };

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
            return Ok(students);
        }

        //Task 4: Get SStudent By Id (GET /api/students/{id})
        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            // Search The list for a stident with the matching ID
            var student = students.FirstOrDefault(s => s.Id == id);

            //If no stuudent is found, return a 404 Not Found response
            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }
            else
            {
                //If a student is found, return the student object with a 200 OK response
                return Ok(student);
            }
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

            //Find all students whose name contains the search string (ignoring uppercase/lowercase)
            var matchingStudents = students
                .Where(s => s.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            //Return the results
            return Ok(matchingStudents);
        }

        // Task 6: Filter Students by Age (GET /api/students/filter-by-age)
        [HttpGet("filter-by-age")]
        public IActionResult FilterByAge()
        {
            //Find students aged 18 to 22 and sort them by age
            var filteredStudents = students
                .Where(s => s.Age >= 18 && s.Age <= 22)
                .OrderBy(s => s.Age)
                .ToList();

            //Return the results
            return Ok(filteredStudents);
        }

        // Task 7: Add New Stuent In Memory (POST /api/students)
        [HttpPost]
        public IActionResult AddStudent([FromBody] Student newStudent)
        {
            //1.Generate a new ID automatically (find the highest current ID and add 1)
            int newId = students.Max(s => s.Id) + 1;

            //2.Assign the new ID to the incoming student data
            newStudent.Id = newId;

            //3. Add the new student to our static list
            students.Add(newStudent);

            //4.Return an OK response with the newly created student
            return Ok(newStudent);
        }

        //Task 8: Edit Student In Memory (PUT /api/students/{id})
        [HttpPut("{id}")]
        public IActionResult EditStudent(int id, [FromBody] Student updatedStudent)
        {
            //1. Fond the exsting studdent in the list
            var existingStudent = students.FirstOrDefault(s => s.Id == id);

            //2. If they don't exist, return Notfound
            if (existingStudent == null)
            {
                return NotFound($"Cannot edit: Student with ID {id} not found.");
            }

            //3. Update their information
            existingStudent.Name = updatedStudent.Name;
            existingStudent.Age = updatedStudent.Age;
            existingStudent.DepartmentId = updatedStudent.DepartmentId;

            //4. Return an OK response with the updated student
            return Ok(existingStudent);
        }

        // Task 9:Delete Student In Memory (DELETE /api/students/{id})
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            //1. Find the existing student in the list
            var student = students.FirstOrDefault(s => s.Id == id);

            //2. If they don't exist, return NotFound
            if (student == null)
            {
                return NotFound($"Cannot delete: Student with ID {id} not found.");
            }
            else
            {
                //3. Remove the student from the list
                students.Remove(student);

                //4. Return an OK response with a message confirming deletion
                return Ok($"Student with ID {id} has been deleted successfully.");
            }
        }
    }
}