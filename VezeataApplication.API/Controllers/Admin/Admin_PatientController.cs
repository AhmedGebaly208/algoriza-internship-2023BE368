using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VezeataApplication.Core.Entities;
using VezeataApplication.Core.Inetrfaces;

namespace VezeataApplication.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("api/Admin/[controller]")]
    [ApiController]
    public class Admin_PatientController : ControllerBase
    {
        private readonly UserManager<VezeataUser> _userManager;
        public Admin_PatientController(UserManager<VezeataUser> userManager)
        {

            _userManager = userManager;
        }

        [HttpGet("GetAllPatient")]
        public async Task<IActionResult> GetAllPatient([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = "")
        {
            page = Math.Max(1, page);
            pageSize = Math.Max(1, pageSize);
            search = search?.Trim().ToLower();
            try
            {
                var result = await _userManager.Users.Where(u => u.AccountType == "Patient").ToListAsync();
                if (!string.IsNullOrEmpty(search))
                {
                    // search on first name , last name or gender
                    result = result.Where(u =>
                    (u.FirstName.ToLower().Contains(search)) ||
                    (u.LastName.ToLower().Contains(search)) ||
                    (u.Gender.ToString().ToLower().Contains(search))
                    ).ToList();
                }
                var response = result.Select(u => new
                {
                    Image = u.Image,
                    FullName = $"{u.FirstName} {u.LastName}",
                    Email = u.Email,
                    Phone = u.PhoneNumber,
                    Gender = u.Gender.ToString(),
                    DateOfBirth = u.DateOfBirth

                });
                return Ok(response);
            }
            catch
            {
                return StatusCode(500, new { Message = "An error occurred while fetching patients." });
            }
        }

        [HttpGet("GetPatientById/{id}")]
        public async Task<IActionResult> GetPatientById([FromRoute] string id)
        {
            try
            {
                var result = await _userManager.Users
                    .Include(u => u.Bookings)
                        .ThenInclude(booking => booking.Times)
                            .ThenInclude(times => times.Appointment)
                                .ThenInclude(appointment => appointment.Doctor)
                                    .ThenInclude(doctor => doctor.User)
                    .Include(u => u.Bookings)
                        .ThenInclude(booking => booking.Times)
                            .ThenInclude(times => times.Appointment)
                                .ThenInclude(appointment => appointment.Doctor)
                                    .ThenInclude(doctor => doctor.Specialization)
                    .Include(u => u.DiscountCodes)
                    .FirstOrDefaultAsync(u => u.Id == id);
                if (result == null)
                {
                    return NotFound($"Not Found Patient with id : {id}");
                }
                var patientDetails = new
                {
                    Image = result.Image,
                    FullName = $"{result.FirstName} {result.LastName}",
                    Email = result.Email,
                    Phone = result.PhoneNumber,
                    Gender = result.Gender,
                    DateOfBirth = result.DateOfBirth
                };

                var bookingRequests = result.Bookings.Select(booking => new
                {
                    Image = booking.Times.Appointment.Doctor.User.Image,
                    DoctorName = $"{booking.Times.Appointment.Doctor.User.FirstName} {booking.Times.Appointment.Doctor.User.LastName}",
                    Specialization = $"{booking.Times.Appointment.Doctor.Specialization.NameEn} - {booking.Times.Appointment.Doctor.Specialization.NameAr}",
                    Day = booking.Times.Appointment.Day,
                    Time = booking.Times.StartTime,
                    Price = booking.Times.Appointment.Doctor.Price,
                    DiscountCode = booking.User.DiscountCodes,
                    FinalPrice = booking.Times.Appointment.Doctor.PriceAfterDiscount,
                    Status = booking.Status
                });
                var response = new
                {
                    Details = patientDetails,
                    BookingRequests = bookingRequests
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"Exception: {ex.Message}");
                return StatusCode(500, new { Message = ex });
            }

        }
    }
}
