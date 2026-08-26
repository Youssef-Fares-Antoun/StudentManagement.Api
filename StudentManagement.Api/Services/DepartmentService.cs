using StudentManagement.Api.Data;
using StudentManagement.Api.Models;

namespace StudentManagement.Api.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _context;

        //Injecting the daatabase context 
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
    }
}