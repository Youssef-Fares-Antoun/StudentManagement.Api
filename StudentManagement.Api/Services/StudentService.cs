using StudentManagement.Api.Dtos;
using StudentManagement.Api.Models;

namespace StudentManagement.Api.Services
{
    // The ": IStudentService" part means this class agrees to follow the contract defined in the interface.
    // It must contain all the methods listed in IStudentService.
    public class StudentService : IStudentService
    {
        // We moved the static lists from the Controller to the Service!
        // These lists act as our temporary "in-memory database" while the application is running.
        private static List<Student> students = new List<Student>
        {
            new Student { Id = 1, Name = "Ahmed", Age = 20, DepartmentId = 1 },
            new Student { Id = 2, Name = "Sara", Age = 22, DepartmentId = 2 },
            new Student { Id = 3, Name = "Omar", Age = 19, DepartmentId = 3 }
        };

        private static List<Department> departments = new List<Department>
        {
            new Department { Id = 1, Name = "IT" },
            new Department { Id = 2, Name = "HR" },
            new Department { Id = 3, Name = "Finance" },
            new Department { Id = 4, Name = "Sales" }
        };

        // Retrieves all students and converts them into DTOs before sending them back.
        public List<StudentDetailsDto> GetAllStudents()
        {
            // .Select() loops through each Student model and transforms it into a StudentDetailsDto.
            return students.Select(s => new StudentDetailsDto
            {
                Id = s.Id,
                Name = s.Name,
                Age = s.Age,
                // We search the departments list to find the matching name. If not found, we default to "Unknown".
                DepartmentName = departments.FirstOrDefault(d => d.Id == s.DepartmentId)?.Name ?? "Unknown"
            }).ToList();
        }

        // Searches for a single student by their ID.
        public StudentDetailsDto? GetStudentById(int id)
        {
            // FirstOrDefault returns the matching student, or null if no student has this ID.
            var student = students.FirstOrDefault(s => s.Id == id);

            // If the student doesn't exist, we immediately return null back to the controller.
            if (student == null) return null;

            // If found, we map the raw Student data into a user-friendly StudentDetailsDto.
            return new StudentDetailsDto
            {
                Id = student.Id,
                Name = student.Name,
                Age = student.Age,
                DepartmentName = departments.FirstOrDefault(d => d.Id == student.DepartmentId)?.Name ?? "Unknown"
            };
        }

        // Finds all students whose name contains the search string.
        public List<StudentDetailsDto> SearchStudents(string name)
        {
            return students
                // .Where filters the list, ignoring uppercase/lowercase differences.
                .Where(s => s.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                // Then we convert the filtered results into DTOs.
                .Select(s => new StudentDetailsDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Age = s.Age,
                    DepartmentName = departments.FirstOrDefault(d => d.Id == s.DepartmentId)?.Name ?? "Unknown"
                })
                .ToList(); // Executes the query and returns it as a List.
        }

        // Returns students aged between 18 and 22, sorted by age.
        public List<StudentDetailsDto> FilterByAge()
        {
            return students
                // Keep only students where Age is >= 18 AND <= 22
                .Where(s => s.Age >= 18 && s.Age <= 22)
                // Sort the remaining students from youngest to oldest
                .OrderBy(s => s.Age)
                // Transform them into DTOs
                .Select(s => new StudentDetailsDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Age = s.Age,
                    DepartmentName = departments.FirstOrDefault(d => d.Id == s.DepartmentId)?.Name ?? "Unknown"
                })
                .ToList();
        }

        // Adds a new student using data from the CreateStudentDto.
        public Student AddStudent(CreateStudentDto dto)
        {
            // 1. Calculate the next available ID by finding the current highest ID and adding 1.
            int newId = students.Max(s => s.Id) + 1;

            // 2. Create a new Student model using the generated ID and the data from the DTO.
            var newStudent = new Student
            {
                Id = newId,
                Name = dto.Name,
                Age = dto.Age,
                DepartmentId = dto.DepartmentId
            };

            // 3. Save the new student to our in-memory list.
            students.Add(newStudent);

            // 4. Return the newly created student model.
            return newStudent;
        }

        // Updates an existing student's data using the UpdateStudentDto.
        public Student? UpdateStudent(int id, UpdateStudentDto dto)
        {
            // 1. Find the exact student we want to edit.
            var existingStudent = students.FirstOrDefault(s => s.Id == id);

            // 2. If they are not in the list, return null so the controller knows to throw a 404 Not Found.
            if (existingStudent == null) return null;

            // 3. Update the existing student's properties with the new data from the DTO.
            existingStudent.Name = dto.Name;
            existingStudent.Age = dto.Age;
            existingStudent.DepartmentId = dto.DepartmentId;

            // 4. Return the updated student.
            return existingStudent;
        }

        // Removes a student from the list based on their ID.
        public bool DeleteStudent(int id)
        {
            // 1. Find the student to delete.
            var student = students.FirstOrDefault(s => s.Id == id);

            // 2. If they don't exist, return false to indicate the deletion failed (404 Not Found).
            if (student == null) return false;

            // 3. Remove them from the list and return true to indicate success.
            students.Remove(student);
            return true;
        }
    }
}