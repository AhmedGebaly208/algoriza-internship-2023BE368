using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VezeataApplication.Core.Entities;
using VezeataApplication.Core.Inetrfaces;
using VezeataApplication.Core.Interfaces;

namespace VezeataApplication.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("api/Admin/[controller]")]
    [ApiController]
    public class Admin_DashbordController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;
        private readonly IAdminService _adminService;
        private readonly IUnitOfWork _unitOfWork;

        public Admin_DashbordController(IDoctorService doctorService, IPatientService patientService,IAdminService adminService,IUnitOfWork unitOfWork)
        {
            _doctorService = doctorService;
            _patientService = patientService;
            _adminService = adminService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("NumOfDoctors")]
        public async Task<IActionResult> NumOfDoctors()
        {
            var numberOfDoctors = await _doctorService.GetNumberOfDoctorAsync();
            return Ok(numberOfDoctors);
        }

        [HttpGet("NumOfPatients")]
        public async Task<IActionResult> NumOfPatients()
        {
            var patients = await _patientService.GetNumberOfPatientAsync();
            return Ok(new { result = $"{patients} patients." });
        }

        [HttpGet("NumOfRequests")]
        public async Task<IActionResult> NumOfRequests()
        {
            var result = await _adminService.GetBookingStatisticsAsync();
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("Top_5_Specilization")]
        public async Task<IActionResult> Top_5_Specilization()
        {
            var result = await _unitOfWork.Specialization.Get_Top_5_SpecializationsAsync();
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("Top_10_Doctors")]
        public async Task<IActionResult> Top_10_Doctors()
        {
            var result = await _doctorService.Get_Top_10_DoctorsAsync();
            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
