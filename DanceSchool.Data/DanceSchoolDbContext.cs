using Microsoft.EntityFrameworkCore;
using DanceSchool.Data.Entities;

namespace DanceSchool.Data
{
    public class DanceSchoolDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Studio> Studios { get; set; }
        public DbSet<StudentGroup> StudentGroups { get; set; }
        public DbSet<GroupInstructor> GroupInstructors { get; set; }
        public DbSet<Class> Classes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=danceschool.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}