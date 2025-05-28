using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
