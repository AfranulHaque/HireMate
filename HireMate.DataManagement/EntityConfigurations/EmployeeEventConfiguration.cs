using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using HireMate.DataManagement.Entities;

namespace HireMate.DataManagement.EntityConfigurations
{
    public class EmployeeEventConfiguration : IEntityTypeConfiguration<EmployeeEvent>
    {
        public void Configure(EntityTypeBuilder<EmployeeEvent> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.EventStartDate)
                   .IsRequired();

            builder.Property(e => e.EventEndDate)
                   .IsRequired();

            // Relationship mapping
            builder.HasOne(e => e.Employee)
                   .WithMany(emp => emp.EmployeeEvents)
                   .HasForeignKey(e => e.EmployeeId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
