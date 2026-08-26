using StudentManagement.Api.Models;

namespace StudentManagement.Api.Services
{
    public interface IDepartmentService
    {
        List<Department> GetAllDepartments();
        Department GetDepartmentById(int id);
        Department AddDepartment(Department department);
        Department UpdateDepartment(int id, Department updatedDepartment);
        bool DeleteDepartment(int id);
    }
}