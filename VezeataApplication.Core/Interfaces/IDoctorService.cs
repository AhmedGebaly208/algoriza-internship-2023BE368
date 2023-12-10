using VezeataApplication.Core.Dto;
using VezeataApplication.Core.Entities;
using VezeataApplication.Core.Models;

namespace VezeataApplication.Core.Interfaces
{
    public interface IDoctorService 
    {
        Task<bool> AddDoctorAsync(DoctorRegistrationModel model);
        Task<bool> UpdateDoctorAsync(int doctorId, DoctorRegistrationModel model);
        Task<bool> DeleteDoctorAsync(int doctorId);
        Task<IEnumerable<PatientDto>> GetDoctorBookingsAsync(string doctorId,int page, int pageSize, string day);
        Task<bool> ConfirmCheckUpAsync(int bookingId);
        Task<int> GetNumberOfDoctorAsync(); 
        Task<IEnumerable<DoctorDto>> GetDoctorsAsync(int page, int pageSize, string search);
        Task<IEnumerable<Doctor>> Get_Top_10_DoctorsAsync();

    }
}
