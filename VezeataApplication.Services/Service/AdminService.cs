using Microsoft.EntityFrameworkCore;
using VezeataApplication.Core.Dto;
using VezeataApplication.Core.Enum;
using VezeataApplication.Core.Interfaces;
using VezeataApplication.Core.Models;

namespace VezeataApplication.Services.Service
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BookingStatsDto> GetBookingStatisticsAsync()
        { 
            var result = await _unitOfWork.Bookings.GetAllAsync();
            var bookingstats = new BookingStatsDto
            {
                requests = result.Count(),
                PendingRequests =result.Where(b=>b.Status == BookingStatus.Pending).Count(),
                CompletedRequests = result.Where(b => b.Status == BookingStatus.Completed).Count(),
                CancelledRequests = result.Where(b => b.Status == BookingStatus.Cancelled).Count()
            };
            return bookingstats;
        }

    }
}
