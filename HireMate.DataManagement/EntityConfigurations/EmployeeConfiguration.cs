using System.Reflection.Emit;
using HireMate.DataManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireMate.DataManagement.EntityConfigurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.EmployeeId);

            builder.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(e => e.Designation)
                .HasMaxLength(100);

            builder.Property(e => e.HireDate)
                .IsRequired();

            builder.Property(e => e.SkillSetDetails)
            .HasMaxLength(1000);

            builder.HasMany(e => e.EmployeeEvents)
                   .WithOne(ev => ev.Employee)
                   .HasForeignKey(ev => ev.EmployeeId)
                   .OnDelete(DeleteBehavior.Cascade); // optional
        }
    }
}
