using StudentManagement.Api.Data;
using StudentManagement.Api.Models;
using StudentManagement.Api.Dtos; 

namespace StudentManagement.Api.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _context;

        //Injecting the database context 
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
            // SQL Server will auto-generate the ID
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
            // Pull the data from the database first
            var departments = _context.Departments.ToList();
            var students = _context.Students.ToList();

            // Calculate the stats in memory using your exact DTO properties
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
            // 1. Reuse the logic we just wrote to get all stats!
            var stats = GetDepartmentStatistics();

            // 2. If there are no departments at all, return an empty DTO
            if (!stats.Any())
            {
                return new DepartmentExtremesDto();
            }

            // 3. Find the maximum and minimum student counts
            int maxStudents = stats.Max(s => s.StudentCount);
            int minStudents = stats.Min(s => s.StudentCount);

            // 4. Find all departments that match those counts (handles ties automatically)
            var highest = stats.Where(s => s.StudentCount == maxStudents).ToList();
            var lowest = stats.Where(s => s.StudentCount == minStudents).ToList();

            // 5. Return the combined result
            return new DepartmentExtremesDto
            {
                Highest = highest,
                Lowest = lowest
            };
        }
    }
}