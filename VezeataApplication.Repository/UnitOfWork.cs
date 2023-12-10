using VezeataApplication.Core.Entities;
using VezeataApplication.Core.Interfaces;
using VezeataApplication.Repository.Repository;

namespace VezeataApplication.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IRepository<Appointment> Appointments { get; private set; }

        public IRepository<Times> Times { get; private set; }

        public IRepository<Booking> Bookings { get; private set; }
        public IRepository<DiscountCodeCoupon> Coupons { get; private set; }
        public IRepository<Doctor> Doctors { get; private set; }

        public ISpecializationRepository Specialization { get; private set; }

        public IDiscountCodeRepository CouponRepository {  get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Appointments = new Repository<Appointment>(_context);
            Times = new Repository<Times>(_context);
            Bookings = new Repository<Booking>(_context);
            Coupons = new Repository<DiscountCodeCoupon>(_context);
            Doctors = new Repository<Doctor>(_context);
            Specialization = new SpecializationRepository(_context);
            CouponRepository = new DiscountCodeRepository(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
