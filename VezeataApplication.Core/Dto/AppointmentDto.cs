using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeataApplication.Core.Enum;

namespace VezeataApplication.Core.Dto
{
    public class AppointmentDto
    {
        public string Day { get; set; }
        public List<TimeDto> Times { get; set; }
    }
}
