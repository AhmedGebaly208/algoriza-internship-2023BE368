using VezeataApplication.Core.Entities;
using VezeataApplication.Core.Inetrfaces;

namespace VezeataApplication.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Appointment> Appointments { get; }
        IRepository<Times> Times { get; }
        IRepository<Booking> Bookings { get; }
        IRepository<DiscountCodeCoupon> Coupons { get; }
        IRepository<Doctor> Doctors { get; }
        ISpecializationRepository Specialization { get; }
        IDiscountCodeRepository CouponRepository { get; }

        Task<int> SaveChangesAsync();
    }
}
