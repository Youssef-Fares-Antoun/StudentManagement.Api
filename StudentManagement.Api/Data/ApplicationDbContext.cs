using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Models; //this line is needed to access ou student and department models

namespace StudentManagement.Api.Data
{
    //Inheriting from DbContent gives this class the power to interact with Sql Server
    public class ApplicationDbContext : DbContext
    {
        //The constructor passes our database configuration to the base EF core class
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        //A DbSet represents a table in your database.
        //We are telling EF core to create a "Students" table and a "Departments" table
        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}