namespace HireMate.DataManagement.Entities
{
    public class EmployeeEvent
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }

        public Employee Employee { get; set; }
    }
}
