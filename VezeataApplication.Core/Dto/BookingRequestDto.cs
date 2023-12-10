using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VezeataApplication.Core.Dto
{
    public class BookingRequestDto
    {
        [Required]
        public int TimeId { get; set; }

        public string DiscountCodeCoupon { get; set; }
    }
}
