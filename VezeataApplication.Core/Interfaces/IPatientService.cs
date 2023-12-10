using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeataApplication.Core.Dto;

namespace VezeataApplication.Core.Interfaces
{
    public interface IPatientService
    {
        Task<int> GetNumberOfPatientAsync();
        Task<bool> MakeBookingAsync(int doctorId, BookingRequestDto bookingRequest);
        Task<bool> CancelBookingAsync(int bookingId);
        Task<IEnumerable<PatientBookingDto>> GetPatientBookingsAsync(string userId);
    }
}
