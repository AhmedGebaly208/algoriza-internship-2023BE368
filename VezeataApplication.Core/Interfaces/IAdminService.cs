using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeataApplication.Core.Dto;

namespace VezeataApplication.Core.Interfaces
{
    public interface IAdminService
    {
        Task<BookingStatsDto> GetBookingStatisticsAsync();
    }
}
