using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeataApplication.Core.Models;

namespace VezeataApplication.Core.Interfaces
{
    public interface IAppointmentService
    {
        Task<bool> AddAppointmentAsync(int doctorId, AppointmentModel model);
        Task<bool> UpdateAppointmentAsync(int doctorId, int timeId, TimeModel model);
        Task<bool> DeleteAppointmentAsync(int doctorId, int timeId);
    }
}
