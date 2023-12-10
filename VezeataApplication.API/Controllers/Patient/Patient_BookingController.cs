using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VezeataApplication.Core.Dto;
using VezeataApplication.Core.Inetrfaces;
using VezeataApplication.Core.Interfaces;

namespace VezeataApplication.API.Controllers.Patient
{
    [Authorize(Roles = "Patient")]
    [Route("api/[controller]")]
    [ApiController]
    public class Patient_BookingController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;

        public Patient_BookingController(IDoctorService doctorService,IPatientService patientService)
        {
            _doctorService = doctorService;
            _patientService = patientService;
        }

        [HttpGet("GetDoctors")]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctors([FromQuery] int page = 1,[FromQuery] int pageSize = 10,[FromQuery] string search = "")
        {
            try
            {
                var doctors = await _doctorService.GetDoctorsAsync(page, pageSize, search);
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("MakeBooking/{doctorId}")]
        public async Task<IActionResult> MakeBooking([FromRoute]int doctorId, [FromBody] BookingRequestDto bookingRequest)
        {
            bool success = await _patientService.MakeBookingAsync(doctorId, bookingRequest);

            if (success)
            {
                return Ok(new { Success = true });
            }

            return BadRequest(new { Success = false, Message = "Booking failed." });
        }

        [HttpPut("CancelBooking/{bookingId}")]
        public async Task<IActionResult> CancelBooking([FromRoute]int bookingId)
        {
            var result = await _patientService.CancelBookingAsync(bookingId);

            if (result)
            {
                return Ok("Booking canceled successfully");
            }

            return NotFound("Booking not found or unable to cancel");
        }

        [HttpGet("GetAllPatientBooking")]
        public async Task<IActionResult> GetPatientBookings()
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var patientBookings = await _patientService.GetPatientBookingsAsync(userId);

            return Ok(patientBookings);
        }
    }
}
