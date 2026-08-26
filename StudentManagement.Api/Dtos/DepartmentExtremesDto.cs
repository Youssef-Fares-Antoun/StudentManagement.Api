namespace StudentManagement.Api.Dtos
{
    public class DepartmentExtremesDto
    {
        public List<DepartmentStatisticsDto> Highest { get; set; } = new List<DepartmentStatisticsDto>();
        public List<DepartmentStatisticsDto> Lowest { get; set; } = new List<DepartmentStatisticsDto>();
    }
}