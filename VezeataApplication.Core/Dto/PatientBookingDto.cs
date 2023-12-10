using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeataApplication.Core.Enum;

namespace VezeataApplication.Core.Dto
{
    public class PatientBookingDto : DoctorDto
    {
        public string Day {  get; set; }
        public string Time { get; set; }
        public string DiscountCode { get; set; }
        public decimal FinalPrice { get; set; }
        public BookingStatus Status { get; set; }
    }
}
