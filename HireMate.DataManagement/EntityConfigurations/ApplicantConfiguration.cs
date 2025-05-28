using HireMate.DataManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireMate.DataManagement.EntityConfigurations
{
    public class ApplicantConfiguration : IEntityTypeConfiguration<Applicant>
    {
        public void Configure(EntityTypeBuilder<Applicant> builder)
        {
            builder.HasKey(a => a.ApplicantId);

            builder.Property(a => a.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(a => a.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(a => a.ResumeFilePath)
                .HasMaxLength(500);

            builder.Property(a => a.ApplicationDate)
                .IsRequired();

            builder.Property(a => a.IsShortlisted)
                .HasDefaultValue(false);

            builder.Property(a => a.IsHired)
                .HasDefaultValue(false);

            builder.HasOne(a => a.JobPost)
                .WithMany()
                .HasForeignKey(a => a.JobPostId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
