using StudentManagement.Api.Models;
using StudentManagement.Api.Dtos;

namespace StudentManagement.Api.Services
{
    public interface IDepartmentService
    {
        List<Department> GetAllDepartments();
        Department GetDepartmentById(int id);
        Department AddDepartment(Department department);
        Department UpdateDepartment(int id, Department updatedDepartment);
        bool DeleteDepartment(int id);

        // Task 12 definition
        List<DepartmentStatisticsDto> GetDepartmentStatistics();

        // Task 13 definition
        DepartmentExtremesDto GetHighestAndLowestDepartments();

        // Task 14 definition
        bool IsDepartmentNameUnique(string name, int currentId = 0);
    }
}