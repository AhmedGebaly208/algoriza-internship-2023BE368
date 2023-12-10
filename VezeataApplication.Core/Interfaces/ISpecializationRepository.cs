using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeataApplication.Core.Dto;
using VezeataApplication.Core.Entities;

namespace VezeataApplication.Core.Interfaces
{
    public interface ISpecializationRepository
    {
        Task<IEnumerable<SpecializationDto>> Get_Top_5_SpecializationsAsync();
        Task<Specialization> GetSpecializationByNameAsync(string specializationName);
    }
}
