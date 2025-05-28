using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireMate.Notification.Unified.Interface
{
    public interface ICalendarAvailabilityChecker
    {
        void GetFreeBusyAsync();
    }
}
