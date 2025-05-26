using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireMate.Notification.Unified.Imp
{
    interface ICalendarAvailabilityChecker
    {
        void GetFreeBusyAsync();
    }
}
