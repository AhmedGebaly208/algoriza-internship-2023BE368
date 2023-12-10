using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VezeataApplication.Core.Enum;
using VezeataApplication.Core.Interfaces;

namespace VezeataApplication.Controllers.Doctors
{
    [Authorize(Roles = "Doctor")]
    [Route("api/Doctor/[controller]")]
    [ApiController]
    public class Doctor_BookingController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public Doctor_BookingController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet("GetAllDoctorsBooking")]
        public async Task<IActionResult> GetAllDoctorsBooking([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string dayOrtime="")
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(doctorId))
            {
                return BadRequest("Doctor not found");
            }
            var result = await _doctorService.GetDoctorBookingsAsync(doctorId,page,pageSize,dayOrtime);
            return Ok(result);
        }

        [HttpPut("ConfirmCheckUp/{bookingId}")]
        public async Task<IActionResult> ConfirmCheckUp([FromRoute] int bookingId)
        {
            var result = await _doctorService.ConfirmCheckUpAsync(bookingId);
            return Ok(result);
        }

    }
}
