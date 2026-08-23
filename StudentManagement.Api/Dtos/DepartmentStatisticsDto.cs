namespace StudentManagement.Api.Dtos
{
    public class DepartmentStatisticsDto
    {
        public string DepartmentName { get; set; }
        public int StudentCount { get; set; }
        public double AverageAge { get; set; }

        // We make these nullable (int?) just in case a department has 0 students!
        public int? OldestAge { get; set; }
        public int? YoungestAge { get; set; }
    }
}
