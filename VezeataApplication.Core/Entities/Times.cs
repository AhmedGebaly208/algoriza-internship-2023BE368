using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VezeataApplication.Core.Entities
{
    public class Times
    {
        public int Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public int AppointmentId { get; set; }
        public virtual Appointment Appointment { get; set; }
        public virtual Booking Booking { get; set; }
    }
}
