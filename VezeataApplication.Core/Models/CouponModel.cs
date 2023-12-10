using System.ComponentModel.DataAnnotations;
using VezeataApplication.Core.Enum;

namespace VezeataApplication.Core.Models
{
    public class CouponModel
    {
        [Required]
        [Length(3,6)]
        public string CouponName { get; set; }
        [Required]
        public int NumOfCompletedRequest { get; set;}
        [Required]
        public DiscountType DiscountType { get; set; }
        [Required]
        public double Value { get; set; }
    }
}
