using HireMate.DataManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireMate.DataManagement.EntityConfigurations
{
    public class JobPostConfiguration : IEntityTypeConfiguration<JobPost>
    {
        public void Configure(EntityTypeBuilder<JobPost> builder)
        {
            builder.HasKey(j => j.JobPostId);

            builder.Property(j => j.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(j => j.Description)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(j => j.Location)
                .HasMaxLength(200);

            builder.Property(j => j.Requirements)
                .HasMaxLength(2000);

            builder.Property(j => j.Benefits)
                .HasMaxLength(1000);

            builder.Property(j => j.CompanyName)
                .HasMaxLength(255);

            builder.Property(j => j.ContactEmail)
                .HasMaxLength(255);

            builder.Property(j => j.PostedDate)
                .IsRequired();

            builder.Property(j => j.IsActive)
                .HasDefaultValue(true);
        }
    }
}
