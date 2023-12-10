using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeataApplication.Core.Entities;
using VezeataApplication.Core.Models;

namespace VezeataApplication.Core.Interfaces
{
    public interface IDiscountCodeService
    {
        Task<bool> AddCouponCodeAsync(CouponModel model);
        Task<bool> UpdateCouponAsync(int id, CouponModel model);
        Task<bool> DeleteCouponAsync(int id);
        Task<bool> DeactivateCouponCodeAsync(int couponId);
    }
}
