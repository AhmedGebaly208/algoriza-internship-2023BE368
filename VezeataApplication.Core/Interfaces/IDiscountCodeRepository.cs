using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeataApplication.Core.Entities;

namespace VezeataApplication.Core.Interfaces
{
    public interface IDiscountCodeRepository
    {
        Task<DiscountCodeCoupon> GetByCodeAsync(string code);
    }
}
