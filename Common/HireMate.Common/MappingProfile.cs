using HireMate.Model.Domain;
using entity = HireMate.DataManagement.Entities;

namespace HireMate.Common
{
    public static class MappingProfile
    {
        public static entity.Employee ToEntity(Employee domain)
        {
            if (domain == null) return null;
            return new entity.Employee
            {
                EmployeeId = domain.EmployeeId,
                FirstName = domain.FirstName,
                LastName = domain.LastName,
                Email = domain.Email,
                PhoneNumber = domain.PhoneNumber,
                Designation = domain.Designation,
                HireDate = domain.HireDate,
                SkillSetDetails = domain.SkillSetDetails
            };
        }

        public static Employee ToDomain(entity.Employee entity)
        {
            if (entity == null) return null;
            return new Employee
            {
                EmployeeId = entity.EmployeeId,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                Designation = entity.Designation,
                HireDate = entity.HireDate,
                SkillSetDetails = entity.SkillSetDetails
            };
        }

        public static entity.Applicant ToEntity(Applicant domain)
        {
            if (domain == null) return null;
            return new entity.Applicant
            {
                ApplicantId = domain.ApplicantId,
                FirstName = domain.FirstName,
                LastName = domain.LastName,
                Email = domain.Email,
                PhoneNumber = domain.PhoneNumber,
                ResumeFilePath = domain.ResumeFilePath,
                ApplicationDate = domain.ApplicationDate,
                IsShortlisted = domain.IsShortlisted,
                IsHired = domain.IsHired,
                JobPostId = domain.JobPostId
            };
        }

        public static Applicant ToDomain(entity.Applicant entity)
        {
            if (entity == null) return null;
            return new Applicant
            {
                ApplicantId = entity.ApplicantId,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                ResumeFilePath = entity.ResumeFilePath,
                ApplicationDate = entity.ApplicationDate,
                IsShortlisted = entity.IsShortlisted,
                IsHired = entity.IsHired,
                JobPostId = entity.JobPostId
            };
        }

        public static entity.JobPost ToEntity(JobPost domain)
        {
            if (domain == null) return null;
            return new entity.JobPost
            {
                JobPostId = domain.JobPostId,
                Title = domain.Title,
                Description = domain.Description,
                Location = domain.Location,
                Requirements = domain.Requirements,
                Benefits = domain.Benefits,
                CompanyName = domain.CompanyName,
                ContactEmail = domain.ContactEmail,
                PostedDate = domain.PostedDate,
                ExpiryDate = domain.ExpiryDate,
                IsActive = domain.IsActive
            };
        }

        public static JobPost ToDomain(entity.JobPost entity)
        {
            if (entity == null) return null;
            return new JobPost
            {
                JobPostId = entity.JobPostId,
                Title = entity.Title,
                Description = entity.Description,
                Location = entity.Location,
                Requirements = entity.Requirements,
                Benefits = entity.Benefits,
                CompanyName = entity.CompanyName,
                ContactEmail = entity.ContactEmail,
                PostedDate = entity.PostedDate,
                ExpiryDate = entity.ExpiryDate,
                IsActive = entity.IsActive
            };
        }
    }
}
