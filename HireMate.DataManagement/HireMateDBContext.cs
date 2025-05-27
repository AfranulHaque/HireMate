using HireMate.DataManagement.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace HireMate.DataManagement
{
    public class HireMateDBContext : DbContext
    {
        public HireMateDBContext(DbContextOptions<HireMateDBContext> options)
            : base(options) { }

        public DbSet<Entities.Employee> Employees { get; set; }
        public DbSet<Entities.JobPost> JobPosts { get; set; }
        public DbSet<Entities.Applicant> Applicants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicantConfiguration());
            modelBuilder.ApplyConfiguration(new JobPostConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
