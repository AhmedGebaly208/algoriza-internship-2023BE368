using VezeataApplication.Core.Enum;

namespace VezeataApplication.Core.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        public int CouponId { get; set; }
        public virtual DiscountCodeCoupon? DiscountCodeCoupon { get; set; }
        public int TimesId { get; set; }
        public virtual Times Times { get; set; }
        public virtual VezeataUser User { get; set; }
    }
}
