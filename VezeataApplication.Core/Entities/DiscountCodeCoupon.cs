using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeataApplication.Core.Enum;

namespace VezeataApplication.Core.Entities
{
    public class DiscountCodeCoupon
    {
        public int Id { get; set; }
        public string CodeName { get; set; }
        public int NumberOfComletedRequesrt { get; set; }
        public DiscountType? DiscountType { get; set; } = null;
        public double Value { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual ICollection<Booking>? Bookings { get; set; }
        public virtual ICollection<VezeataUser>? VezeataUsers { get; set; }
    }
}
