using StudentManagement.Api.Data;
using StudentManagement.Api.Models;
using StudentManagement.Api.Dtos;

namespace StudentManagement.Api.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _context;

        public DepartmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Department> GetAllDepartments()
        {
            return _context.Departments.ToList();
        }

        public Department GetDepartmentById(int id)
        {
            return _context.Departments.FirstOrDefault(d => d.Id == id);
        }

        public Department AddDepartment(Department department)
        {
            _context.Departments.Add(department);
            _context.SaveChanges();
            return department;
        }

        public Department UpdateDepartment(int id, Department updatedDepartment)
        {
            var existing = _context.Departments.FirstOrDefault(d => d.Id == id);
            if (existing == null) return null;

            existing.Name = updatedDepartment.Name;

            _context.SaveChanges();
            return existing;
        }

        public bool DeleteDepartment(int id)
        {
            var existing = _context.Departments.FirstOrDefault(d => d.Id == id);
            if (existing == null) return false;

            _context.Departments.Remove(existing);
            _context.SaveChanges();
            return true;
        }

        // Task 12: Department Statistics
        public List<DepartmentStatisticsDto> GetDepartmentStatistics()
        {
            var departments = _context.Departments.ToList();
            var students = _context.Students.ToList();

            return departments.Select(d => {
                var deptStudents = students.Where(s => s.DepartmentId == d.Id).ToList();

                return new DepartmentStatisticsDto
                {
                    DepartmentName = d.Name,
                    StudentCount = deptStudents.Count,
                    AverageAge = deptStudents.Any() ? deptStudents.Average(s => s.Age) : 0,
                    OldestAge = deptStudents.Any() ? deptStudents.Max(s => s.Age) : null,
                    YoungestAge = deptStudents.Any() ? deptStudents.Min(s => s.Age) : null
                };
            }).ToList();
        }

        // Task 13: Highest and Lowest Department
        public DepartmentExtremesDto GetHighestAndLowestDepartments()
        {
            var stats = GetDepartmentStatistics();

            if (!stats.Any())
            {
                return new DepartmentExtremesDto();
            }

            int maxStudents = stats.Max(s => s.StudentCount);
            int minStudents = stats.Min(s => s.StudentCount);

            var highest = stats.Where(s => s.StudentCount == maxStudents).ToList();
            var lowest = stats.Where(s => s.StudentCount == minStudents).ToList();

            return new DepartmentExtremesDto
            {
                Highest = highest,
                Lowest = lowest
            };
        }

        // Task 14: Check for Duplicate Department Names
        public bool IsDepartmentNameUnique(string name, int currentId = 0)
        {
            // If currentId is 0 (adding), it checks if the name exists anywhere.
            // If currentId > 0 (updating), it checks if the name belongs to a DIFFERENT department.
            return !_context.Departments.Any(d => d.Name.ToLower() == name.ToLower() && d.Id != currentId);
        }
    }
}