namespace HireMate.DataManagement.Entities
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Designation { get; set; }
        public DateTime HireDate { get; set; }
        public string? SkillSetDetails { get; set; }

        public ICollection<EmployeeEvent> EmployeeEvents { get; set; } = new List<EmployeeEvent>();

    }
}
