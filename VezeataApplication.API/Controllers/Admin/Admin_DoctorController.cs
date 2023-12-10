using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VezeataApplication.Core.Entities;
using VezeataApplication.Core.Enum;
using VezeataApplication.Core.Interfaces;
using VezeataApplication.Core.Models;

namespace VezeataApplication.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("api/Admin/[controller]")]
    [ApiController]
    public class Admin_DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly IUnitOfWork _unitOfWork;
        public Admin_DoctorController(IDoctorService doctorService, IUnitOfWork unitOfWork)
        {
            _doctorService = doctorService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllDoctors")]
        public async Task<IActionResult> GetAllDoctors([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = "")
        {
            page = Math.Max(1, page);
            pageSize = Math.Max(1, pageSize);
            search = search?.Trim().ToLower();

            var result = await _doctorService.GetDoctorsAsync(page, pageSize, search);

            var response = result.Select(d => new
            {
                Image = d.Image,
                FullName = d.FullName,
                Email = d.Email,
                Phone = d.Phone,
                Gender = d.Gender,
                Specialize = d.Specialize

            });

            return Ok(response);
        }

        [HttpGet("GetDoctorById/{id}")]
        public async Task<IActionResult> GetDoctorById([FromRoute] int id)
        {

            var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);
            if (doctor == null)
            {
                return NotFound(new { Message = $"Doctor with ID {id} not found." });
            }


            var response = new
            {
                Details = new
                {
                    Image = doctor.User.Image,
                    FullName = $"{doctor.User.FirstName} {doctor.User.LastName}",
                    Email = doctor.User.Email,
                    Phone = doctor.User.PhoneNumber,
                    Specialization = $"{doctor.Specialization.NameEn}-{doctor.Specialization.NameAr}",
                    Gender = doctor.User.Gender.ToString(),
                    DateOfBirh = doctor.User.DateOfBirth
                }
            };

            return Ok(response);
        }

        [HttpPost("AddDoctor")]
        public async Task<IActionResult> AddDoctor([FromBody] DoctorRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _doctorService.AddDoctorAsync(model);

            return Ok(result);
        }

        [HttpPut("EditDoctor/{id}")]
        public async Task<IActionResult> EditDoctor([FromRoute] int id, [FromBody] DoctorRegistrationModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _doctorService.UpdateDoctorAsync(id, model);
            if(result == false)
                return StatusCode(500 , new {Massege = $"Doctor with id : {id} not found."});

            return Ok(result);

        }

        [HttpDelete("DeleteDoctor/{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var result = await _doctorService.DeleteDoctorAsync(id);
            return Ok(result);
        }

    }
}
