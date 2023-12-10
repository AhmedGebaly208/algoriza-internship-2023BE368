using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeataApplication.Core.Dto;
using VezeataApplication.Core.Entities;
using VezeataApplication.Core.Enum;
using VezeataApplication.Core.Inetrfaces;
using VezeataApplication.Core.Interfaces;

namespace VezeataApplication.Services.Service
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<VezeataUser> _userManager;

        public PatientService(IUnitOfWork unitOfWork, UserManager<VezeataUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<int> GetNumberOfPatientAsync()
        {
            var result = await _userManager.Users.Where(u=>u.AccountType.ToLower().Contains("patient")).CountAsync();
            return result;
        }
        public async Task<bool> MakeBookingAsync(int doctorId, BookingRequestDto bookingRequest)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(doctorId);

            if (doctor == null)
            {
                // Doctor not found
                return false;
            }

            // Check if the time slot is available
            var time = await _unitOfWork.Times.GetByIdAsync(bookingRequest.TimeId);

            if (time == null || time.AppointmentId != doctor.Appointments[0].Id)
            {
                // Time slot not found or not associated with the doctor's appointment
                return false;
            }

            // Check discount code if provided
            if (!string.IsNullOrEmpty(bookingRequest.DiscountCodeCoupon))
            {
                var discountCode = await _unitOfWork.CouponRepository.GetByCodeAsync(bookingRequest.DiscountCodeCoupon);

                if (discountCode == null || !discountCode.IsActive)
                {
                    // Invalid or inactive discount code
                    return false;
                }

                // Increment the completed requests count for the discount code
                discountCode.NumberOfComletedRequesrt++;
            }

            // Create the booking
            var booking = new Booking
            {
                Status = BookingStatus.Pending,
                TimesId = bookingRequest.TimeId,
                //User = ,
                //DiscountCodeCoupon = ,
            };

            // Add the booking to the database
            await _unitOfWork.Bookings.AddAsync(booking);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId);

            if (booking == null)
                return false; // Booking not found
            

            if (booking.Status == BookingStatus.Cancelled || booking.Status == BookingStatus.Completed)
                return false;

            // update cancel status from pending to cancelled
            booking.Status = BookingStatus.Cancelled;
            
            // Save changes to the database
            await _unitOfWork.SaveChangesAsync();

            return true; // Booking successfully canceled
        }
        public async Task<IEnumerable<PatientBookingDto>> GetPatientBookingsAsync(string userId)
        {
            var patientBookings = await _unitOfWork.Bookings.GetAllAsync();
            var result = patientBookings.Where(b => b.User.Id == userId).ToList();
            return result.Select(b => new PatientBookingDto
            {
                Image = b.Times.Appointment.Doctor.User.Image,
                FullName = $"{b.Times.Appointment.Doctor.User.FirstName} {b.Times.Appointment.Doctor.User.LastName}",
                Specialize = $"{b.Times.Appointment.Doctor.Specialization.NameEn} - {b.Times.Appointment.Doctor.Specialization.NameAr}" , 
                Day = b.Times.Appointment.Day.ToString(),
                Time = b.Times.StartTime.ToString("hh:mm tt"),
                Price = b.Times.Appointment.Doctor.Price ?? 0, // if Price is nullable
                DiscountCode = b.DiscountCodeCoupon?.CodeName,
                FinalPrice = b.Times.Appointment.Doctor.PriceAfterDiscount ?? b.Times.Appointment.Doctor.Price ?? 0,
                Status = b.Status
            });
        }
    }
}
