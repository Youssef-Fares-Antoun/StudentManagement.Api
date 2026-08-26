using StudentManagement.Api.Dtos;
using StudentManagement.Api.Models;
using StudentManagement.Api.Controllers;
using StudentManagement.Api.Data;

namespace StudentManagement.Api.Services
{
    // The ": IStudentService" part means this class agrees to follow the contract defined in the interface.
    // It must contain all the methods listed in IStudentService.
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;

        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Retrieves all students and converts them into DTOs before sending them back.
        public List<StudentDetailsDto> GetAllStudents()
        {
            var allStudents = _context.Students.ToList();
            var allDepartments = _context.Departments.ToList();

            return allStudents.Select(s => new StudentDetailsDto
            {
                Id = s.Id,
                Name = s.Name,
                Age = s.Age,
                DepartmentName = allDepartments.FirstOrDefault(d => d.Id == s.DepartmentId)?.Name ?? "Unknown"
            }).ToList();
        }

        public StudentDetailsDto GetStudentById(int id)
        {
            var student = _context.Students.FirstOrDefault(s => s.Id == id);
            if (student == null) return null;

            var department = _context.Departments.FirstOrDefault(d => d.Id == student.DepartmentId);

            return new StudentDetailsDto
            {
                Id = student.Id,
                Name = student.Name,
                Age = student.Age,
                DepartmentName = department?.Name ?? "Unknown"
            };
        }

        // Finds all students whose name contains the search string.
        public List<StudentDetailsDto> SearchStudents(string name)
        {
            // 1. Get the filtered students from the database first
            var filteredStudents = _context.Students
                .Where(s => s.Name.Contains(name)) // Removed StringComparison because SQL Server handles case-insensitivity automatically
                .ToList();

            // 2. Get departments for mapping
            var allDepartments = _context.Departments.ToList();

            // 3. Map to DTOs in memory (where the ?. operator is allowed)
            return filteredStudents.Select(s => new StudentDetailsDto
            {
                Id = s.Id,
                Name = s.Name,
                Age = s.Age,
                DepartmentName = allDepartments.FirstOrDefault(d => d.Id == s.DepartmentId)?.Name ?? "Unknown"
            }).ToList();
        }

        // Returns students aged between 18 and 22, sorted by age.
        public List<StudentDetailsDto> FilterByAge()
        {
            // 1. Get the filtered and sorted students from the database first
            var filteredStudents = _context.Students
                .Where(s => s.Age >= 18 && s.Age <= 22)
                .OrderBy(s => s.Age)
                .ToList();

            // 2. Get departments for mapping
            var allDepartments = _context.Departments.ToList();

            // 3. Map to DTOs in memory
            return filteredStudents.Select(s => new StudentDetailsDto
            {
                Id = s.Id,
                Name = s.Name,
                Age = s.Age,
                DepartmentName = allDepartments.FirstOrDefault(d => d.Id == s.DepartmentId)?.Name ?? "Unknown"
            }).ToList();
        }

        // Adds a new student using data from the CreateStudentDto.
        public Student AddStudent(CreateStudentDto dto)
        {


            // 2. Create a new Student model using the generated ID and the data from the DTO.
            var newStudent = new Student
            {
                Name = dto.Name,
                Age = dto.Age,
                DepartmentId = dto.DepartmentId
            };

            _context.Students.Add(newStudent);
            _context.SaveChanges();

            // 4. Return the newly created student model.
            return newStudent;
        }

        // Updates an existing student's data using the UpdateStudentDto.
        public Student? UpdateStudent(int id, UpdateStudentDto dto)
        {

            var existingStudent = _context.Students.FirstOrDefault(s => s.Id == id);
            // 2. If they are not in the list, return null so the controller knows to throw a 404 Not Found.
            if (existingStudent == null) return null;

            // 3. Update the existing student's properties with the new data from the DTO.
            existingStudent.Name = dto.Name;
            existingStudent.Age = dto.Age;
            existingStudent.DepartmentId = dto.DepartmentId;

            _context.SaveChanges();

            // 4. Return the updated student.
            return existingStudent;
        }

        // Removes a student from the list based on their ID.
        public bool DeleteStudent(int id)
        {
            // 1. Find the student first!
            var existingStudent = _context.Students.FirstOrDefault(s => s.Id == id);

            if (existingStudent == null) return false;

            // 2. Remove and save
            _context.Students.Remove(existingStudent);
            _context.SaveChanges();
            return true;
        }
    }
}