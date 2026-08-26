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
    }
}