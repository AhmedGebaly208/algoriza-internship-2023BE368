using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VezeataApplication.Core.Interfaces;
using VezeataApplication.Core.Models;

namespace VezeataApplication.API.Controllers.Doctors
{
    [Authorize(Roles = "Doctor")]
    [Route("api/Doctor/[controller]")]
    [ApiController]
    public class Doctor_SettingController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IUserAccessor _userAccessor;

        public Doctor_SettingController(IAppointmentService appointmentService, IUserAccessor userAccessor)
        {
            _appointmentService = appointmentService;
            _userAccessor = userAccessor;
        }
        [HttpPost("AddAppointment/{doctorId}")]
        public async Task<IActionResult> AddAppointment([FromRoute] int doctorId, [FromBody] AppointmentModel model)
        {
            //var loggedInDoctorId = _userAccessor.GetLoggedInId();

            //// Ensure that the appointment is added for the logged-in doctor
            //if (loggedInDoctorId == null )
            //{
            //    return Unauthorized();
            //}
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _appointmentService.AddAppointmentAsync(doctorId, model);
            return Ok(result);
        }
        [HttpPut("UpdateAppointment/{doctorId}")]
        public async Task<IActionResult> UpdateAppointment([FromRoute] int doctorId, [FromQuery] int timeId, [FromBody] TimeModel model)
        {
            var loggedInDoctorId = _userAccessor.GetLoggedInId();

            // Ensure that the appointment is added for the logged-in doctor
            if (loggedInDoctorId == null || loggedInDoctorId.Value != doctorId)
            {
                return Unauthorized();
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _appointmentService.UpdateAppointmentAsync(doctorId, timeId, model);
            return Ok(result);
        }
        [HttpDelete("DeleteAppoitment/{doctorId}")]
        public async Task<IActionResult> DeleteAppointment([FromRoute] int doctorId, [FromQuery] int timeId)
        {
            var loggedInDoctorId = _userAccessor.GetLoggedInId();

            // Ensure that the appointment is added for the logged-in doctor
            if (loggedInDoctorId == null || loggedInDoctorId.Value != doctorId)
            {
                return Unauthorized();
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _appointmentService.DeleteAppointmentAsync(doctorId, timeId);
            return Ok(result);
        }
    }
}