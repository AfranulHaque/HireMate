namespace HireMate.Common
{
    public class JobPost
    {
        public int JobPostId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string? Requirements { get; set; }
        public string? Benefits { get; set; }
        public string? CompanyName { get; set; }
        public string? ContactEmail { get; set; }
        public DateTime PostedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public string? ProcessInfo { get; set; }
    }
}
