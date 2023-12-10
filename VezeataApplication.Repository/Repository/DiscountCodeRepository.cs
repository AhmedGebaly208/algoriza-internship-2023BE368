using Microsoft.EntityFrameworkCore;
using VezeataApplication.Core.Entities;
using VezeataApplication.Core.Interfaces;

namespace VezeataApplication.Repository.Repository
{
    public class DiscountCodeRepository : Repository<DiscountCodeCoupon>, IDiscountCodeRepository
    {
        public DiscountCodeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<DiscountCodeCoupon> GetByCodeAsync(string code)
        {
            return await _context.DiscountCodeCoupons.FirstOrDefaultAsync(dc => dc.CodeName == code);

        }
    }
}
