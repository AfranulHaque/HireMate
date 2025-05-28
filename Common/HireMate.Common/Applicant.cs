namespace HireMate.Common
{
    public class Applicant
    {
        public int ApplicantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? ResumeFilePath { get; set; }
        public DateTime ApplicationDate { get; set; }
        public bool IsShortlisted { get; set; } = false;
        public bool IsHired { get; set; } = false;
        public int JobPostId { get; set; }
    }
}
