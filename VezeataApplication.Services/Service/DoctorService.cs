using VezeataApplication.Core.Dto;
using VezeataApplication.Core;
using VezeataApplication.Core.Entities;
using VezeataApplication.Core.Enum;
using VezeataApplication.Core.Interfaces;
using VezeataApplication.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace VezeataApplication.Services.Service
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<VezeataUser> _userManager;

        public DoctorService(IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager, UserManager<VezeataUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<bool> AddDoctorAsync(DoctorRegistrationModel model)
        {
            var userEmail = await _userManager.FindByEmailAsync(model.Email);
            var userName = await _userManager.FindByNameAsync(model.UserName);
            if (userEmail != null || userName != null)
            {
                return false;
            }


            var role = await _roleManager.FindByNameAsync("Doctor");
            var doctoruser = new VezeataUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Image = model.Image,
                Gender = model.Gender,
                PhoneNumber = model.Phone,
                DateOfBirth = model.DateOfBirth,
                AccountType = role.Name
            };
            var result = await _userManager.CreateAsync(doctoruser, model.Password);
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description}, ";
                }
                return false;
            }
            await _userManager.AddToRoleAsync(doctoruser, "Doctor");
            var doctor = new Doctor
            {
                User = doctoruser
            };
            var specialization = await _unitOfWork.Specialization.GetSpecializationByNameAsync(model.SpecializationName);
            if (specialization == null)
                return false;
            doctor.Specialization = specialization;
            await _unitOfWork.Doctors.AddAsync(doctor);
            await _unitOfWork.SaveChangesAsync();

            return true; // Assuming the doctor was added successfully
        }

        public async Task<bool> UpdateDoctorAsync(int doctorId, DoctorRegistrationModel model)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(doctorId);

            if ( doctor == null)
            {
                return false;
            }
 

            doctor.User.Image = model.Image;
            doctor.User.FirstName = model.FirstName;
            doctor.User.LastName = model.LastName;
            doctor.User.Email = model.Email;
            doctor.User.PhoneNumber = model.Phone;
            doctor.User.Gender = model.Gender;
            doctor.User.DateOfBirth = model.DateOfBirth;

            var newSpecialization = await _unitOfWork.Specialization.GetSpecializationByNameAsync(model.SpecializationName);
            if (newSpecialization != null)
                doctor.Specialization = newSpecialization;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeleteDoctorAsync(int doctorId)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(doctorId);
            // Check if the doctor has any booking requests
            var hasBookingRequests = await _unitOfWork.Bookings.GetAllAsync();
            var result = hasBookingRequests.Any(b => b.Times.Appointment.DoctorId == doctorId);

            if (doctor == null || result)
            {
                return false; // Doctor not found or hase any booking request
            }

            // Delete the doctor
            var resultt = await _userManager.DeleteAsync(doctor.User);
            await _unitOfWork.Doctors.Remove(doctorId);

            await _unitOfWork.SaveChangesAsync();

            return true; // Doctor deleted successfully
        }

        public async Task<IEnumerable<DoctorDto>> GetDoctorsAsync(int page, int pageSize, string search)
        {
            page = Math.Max(1, page);
            pageSize = Math.Max(1, pageSize);
            search = search?.Trim().ToLower();

            var query = await _unitOfWork.Doctors.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(d =>
                    d.User.FirstName.ToLower().Contains(search) ||
                    d.User.LastName.ToLower().Contains(search) ||
                    d.User.Email.ToLower().Contains(search)||
                    d.Specialization.NameEn.ToLower().Contains(search) ||
                    d.Specialization.NameAr.Contains(search)
                );
            }
            var doctors = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = doctors.Select(MapToDoctorDto);

            return result;
        }

        public async Task<IEnumerable<PatientDto>> GetDoctorBookingsAsync(string id, int page, int pageSize, string day)
        {
            page = Math.Max(1, page);
            pageSize = Math.Max(1, pageSize);
            day = day?.Trim().ToLower();
            var result = await _unitOfWork.Bookings.GetAllAsync();
            var bookings = result.Where(b =>
                ((b.Times.StartTime.ToString().Contains(day)) || (b.Times.Appointment.Day.ToString().ToLower().Contains(day))) &&
                (b.Status == BookingStatus.Pending));
            if (bookings.Any())
                return null;
            var patient = bookings.Select(b => new PatientDto
            {
                Image = b.User.Image,
                PatientName = $"{b.User.FirstName} {b.User.LastName}",
                Email = b.User.Email,
                Phone = b.User.PhoneNumber,
                Gender = b.User.Gender,
                age = DateTime.Now.Year - b.User.DateOfBirth.Value.Year,
                Apointment = $"{b.Times.Appointment.Day} - {b.Times.StartTime}"
            });
            return patient;

        }
        public async Task<bool> ConfirmCheckUpAsync(int bookingId)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId);

            if (booking == null || booking.Status == BookingStatus.Completed || booking.Status == BookingStatus.Cancelled)
            {
                return false; // Booking not found
            }

            // Confirm the booking
            booking.Status = BookingStatus.Completed;

            // Save changes to the database
            await _unitOfWork.SaveChangesAsync();

            return true; // Booking confirmed successfully
        }

        public async Task<IEnumerable<Doctor>> Get_Top_10_DoctorsAsync()
        {
            var result = await _unitOfWork.Bookings.GetAllAsync();

            var Top10Doctors = result.Where(b => b.Status == BookingStatus.Completed)
               .GroupBy(a => a.Times.Appointment.Doctor)
               .OrderByDescending(g => g.Count())
               .Take(10)
               .Select(g => g.Key)
               .ToList();
            return Top10Doctors;
        }

        public async Task<int> GetNumberOfDoctorAsync()
        {
            var result = await _unitOfWork.Doctors.GetCountAsync();
            return result;
        }

        private DoctorDto MapToDoctorDto(Doctor doctor)
        {
            // Mapping logic from Doctor entity to DoctorDto
            return new DoctorDto
            {
                Image = doctor.User.Image,
                FullName = $"{doctor.User.FirstName} {doctor.User.LastName}",
                Email = doctor.User.Email,
                Phone = doctor.User.PhoneNumber,
                Specialize = doctor.Specialization.NameEn,
                Price = doctor.Price,
                Gender = doctor.User.Gender,
                Appointments = doctor.Appointments.Select(MapToAppointmentDto).ToList()
            };
        }

        private AppointmentDto MapToAppointmentDto(Appointment appointment)
        {
            string name = appointment.Day.ToString();
            // Mapping logic from Appointment entity to AppointmentDto
            return new AppointmentDto
            {
                Day = name,
                Times = appointment.Times.Select(t => new TimeDto { Id = t.Id, Time = t.StartTime }).ToList()
            };
        }
    }
}

