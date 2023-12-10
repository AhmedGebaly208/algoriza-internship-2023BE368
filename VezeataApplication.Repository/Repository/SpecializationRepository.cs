using Microsoft.EntityFrameworkCore;
using VezeataApplication.Core.Dto;
using VezeataApplication.Core.Entities;
using VezeataApplication.Core.Interfaces;
using VezeataApplication.Core.Models;

namespace VezeataApplication.Repository.Repository
{
    public class SpecializationRepository : Repository<Specialization>, ISpecializationRepository
    {
        private readonly ApplicationDbContext _context;

        public SpecializationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SpecializationDto>> Get_Top_5_SpecializationsAsync()
        {
            var topSpecializations = await _context.Specializations
            .OrderByDescending(s => s.Doctors
                .SelectMany(d => d.Appointments)
                .SelectMany(a => a.Times)
                .Select(t => t.Booking)
                .Count())
            .Take(5)
            .Select(s => new SpecializationDto
            {
                FullName = $"{s.NameEn} - {s.NameAr}",
                NumOfRequests = s.Doctors
                .SelectMany(d => d.Appointments)
                .SelectMany(a => a.Times)
                .Select(t => t.Booking).Count()
            }).ToListAsync();

            return topSpecializations;
        }
        public async Task<Specialization> GetSpecializationByNameAsync(string specializationName)
        {
            var result = await _context.Specializations
           .FirstOrDefaultAsync(s => (s.NameAr.Contains(specializationName) || s.NameEn.Contains(specializationName)));

            return result;
        }
    }
}
