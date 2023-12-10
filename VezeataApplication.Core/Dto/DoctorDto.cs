using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeataApplication.Core.Enum;

namespace VezeataApplication.Core.Dto
{
    public class DoctorDto
    {
        public string Image { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Specialize { get; set; }
        public decimal? Price { get; set; }
        public Gender? Gender { get; set; }
        public List<AppointmentDto> Appointments { get; set; }
    }
}
