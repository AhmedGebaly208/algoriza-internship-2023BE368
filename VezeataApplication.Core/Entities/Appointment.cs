using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using VezeataApplication.Core.Enum;

namespace VezeataApplication.Core.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public Days Day { get; set; }
        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual List<Times> Times { get; set; }


    }
}
