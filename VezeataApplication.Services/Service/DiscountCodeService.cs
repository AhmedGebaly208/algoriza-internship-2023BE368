using VezeataApplication.Core.Entities;
using VezeataApplication.Core.Interfaces;
using VezeataApplication.Core.Models;

namespace VezeataApplication.Services.Service
{
    public class DiscountCodeService : IDiscountCodeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DiscountCodeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddCouponCodeAsync(CouponModel model)
        {
            var discountCode = new DiscountCodeCoupon
            {
                CodeName = model.CouponName,
                NumberOfComletedRequesrt = model.NumOfCompletedRequest,
                DiscountType = model.DiscountType,
                Value = model.Value,
                IsActive = true
            };

            await _unitOfWork.Coupons.AddAsync(discountCode);

            // Save changes to the database
            await _unitOfWork.SaveChangesAsync();

            return true; // Assuming the coupon code was added successfully
        }

        public async Task<bool> UpdateCouponAsync(int id, CouponModel model)
        {
            var coupon = await _unitOfWork.Coupons.GetByIdAsync(id);
            var bookings = await _unitOfWork.Bookings.GetAllAsync();
            var result = bookings.Any(b => b.CouponId == coupon.Id);
            if (coupon == null || result)
                return false;

            coupon.CodeName = model.CouponName;
            coupon.NumberOfComletedRequesrt = model.NumOfCompletedRequest;
            coupon.DiscountType = model.DiscountType;
            coupon.Value = model.Value;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCouponAsync(int id)
        {
            var coupon = await _unitOfWork.Coupons.GetByIdAsync(id);
            if (coupon == null || coupon.Bookings.Any(b => b.CouponId == coupon.Id))
                return false;
            

            await _unitOfWork.Coupons.Remove(id);
            await _unitOfWork.SaveChangesAsync();
            return true;

        }

        public async Task<bool> DeactivateCouponCodeAsync(int couponId)
        {
            var coupon = await _unitOfWork.Coupons.GetByIdAsync(couponId);

            if (coupon == null)
            {
                return false; // Coupon not found
            }

            // Deactivate the coupon code
            coupon.IsActive = false;

            // Save changes to the database
            await _unitOfWork.SaveChangesAsync();

            return true; // Coupon code deactivated successfully
        }


    }
}
