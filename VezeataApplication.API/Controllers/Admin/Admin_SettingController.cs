using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VezeataApplication.Core.Entities;
using VezeataApplication.Core.Interfaces;
using VezeataApplication.Core.Models;
using VezeataApplication.Repository;

namespace VezeataApplication.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("api/Admin/[controller]")]
    [ApiController]
    public class Admin_SettingController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDiscountCodeService _discountCodeService;

        public Admin_SettingController(IUnitOfWork unitOfWork,IDiscountCodeService discountCodeService)
        {
            _unitOfWork = unitOfWork;
            _discountCodeService = discountCodeService;
        }

        [HttpPost("AddCoupon")]
        public async Task<IActionResult> AddCoupon([FromBody] CouponModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _discountCodeService.AddCouponCodeAsync(model);
            return Ok(new { result, Message = "Add coupon successful" });

        }

        [HttpPut("UpdateCoupon/{id}")]
        public async Task<IActionResult> UpdateCoupon([FromRoute] int id, [FromBody] CouponModel model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _discountCodeService.UpdateCouponAsync(id, model);
            if(!result)
                return BadRequest(new {result , Maeesge = "Sorry, can't update because Coupon Id nout found or coupon apply on booking request"});
            return Ok(new { result, Message = "Update coupon successfuly" });

        }

        [HttpDelete("DeleteCoupon/{id}")]
        public async Task<IActionResult> DeleteCoupon([FromRoute] int id)
        {
           var result =  await _discountCodeService.DeleteCouponAsync(id);
            return Ok(result);
        }

        [HttpPut("DeActivateCoupon/{id}")]
        public async Task<IActionResult> DeactivateCouponCode([FromRoute] int id)
        {
            var result = await _discountCodeService.DeactivateCouponCodeAsync(id);

            if (result)
            {
                return Ok("Coupon code deactivated successfully");
            }

            return NotFound("Coupon not found or deactivation failed");
        }
    }




}


