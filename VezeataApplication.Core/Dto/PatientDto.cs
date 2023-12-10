using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeataApplication.Core.Enum;

namespace VezeataApplication.Core.Dto
{
    public class PatientDto
    {
        public string PatientName { get; set; }
        public string Image { get; set; }
        public int age { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Gender? Gender { get; set; }
        public string Apointment { get; set; }
    }
}
