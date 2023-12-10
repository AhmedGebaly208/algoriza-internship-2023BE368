using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VezeataApplication.Core.Enum
{
   public enum BookingStatus
    {
        Pending,    // The booking is awaiting confirmation
        Completed,  // The booking has been completed
        Cancelled   // The booking has been cancelled
    }
}
