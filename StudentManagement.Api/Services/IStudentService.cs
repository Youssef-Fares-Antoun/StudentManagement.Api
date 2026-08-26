using StudentManagement.Api.Dtos;
using StudentManagement.Api.Models;

namespace StudentManagement.Api.Services
{
    public interface IStudentService
    {
        List<StudentDetailsDto> GetAllStudents();
        StudentDetailsDto? GetStudentById(int id);
        List<StudentDetailsDto> SearchStudents(string text);
        List<StudentDetailsDto> FilterByAge();
        Student AddStudent(CreateStudentDto dto);
        //The ? Below means that the method is allowed to return null
        Student? UpdateStudent(int id, UpdateStudentDto dto);
        bool DeleteStudent(int id);


    }
}
