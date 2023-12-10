using Microsoft.AspNetCore.Identity;
using VezeataApplication.Core.Dto;
using VezeataApplication.Core.Entities;
using VezeataApplication.Core.Interfaces;
using VezeataApplication.Core.Models;

namespace VezeataApplication.Services.Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<VezeataUser> _userManager;
        private readonly IUserAccessor _userAccessor;

        public AppointmentService(IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager, UserManager<VezeataUser> userManager, IUserAccessor userAccessor)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _userManager = userManager;
            _userAccessor = userAccessor;
        }

        public async Task<bool> AddAppointmentAsync(int doctorId ,AppointmentModel model)
        {
            var timee = await _unitOfWork.Times.GetAllAsync();
            // Check if the doctor has already added an appointment at the same time
            if (timee.Any(t => t.Appointment.DoctorId != doctorId && t.StartTime == model.Days.First().Times.First().StartTime))
            {
                // Doctor has already added an appointment at the same time
                return false;
            }
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(doctorId);
            // update doctor price 
            doctor.Price = model.Price;

            foreach (var dayWithTime in model.Days)
            {
                var newAppointment = new Appointment
                {
                    DoctorId = doctor.Id,
                    Day = dayWithTime.Days,
                    Times = new List<Times>()
                };

                foreach (var time in dayWithTime.Times)
                {
                    var timeOfDay = new Times
                    {
                        StartTime = time.StartTime,
                        Appointment = newAppointment
                    };
                    newAppointment.Times.Add(timeOfDay);
                }
                await _unitOfWork.Appointments.AddAsync(newAppointment);

            }
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAppointmentAsync(int doctorId, int timeId, TimeModel model)
        {

            var time = await _unitOfWork.Times.GetByIdAsync(timeId);

            if (time == null || time.Appointment.DoctorId != doctorId || time.Booking.TimesId == timeId)
                return false;

            
                time.StartTime = model.StartTime;
                await _unitOfWork.SaveChangesAsync();
                return true;

        }

        public async Task<bool> DeleteAppointmentAsync(int doctorId, int timeId)
        {

            var time = await _unitOfWork.Times.GetByIdAsync(timeId);

            if (time == null || time.Appointment.DoctorId != doctorId || time.Booking.TimesId == timeId)
                return false;


            await _unitOfWork.Times.Remove(timeId);
            await _unitOfWork.SaveChangesAsync();
            return true;

        }
 
    }
}
