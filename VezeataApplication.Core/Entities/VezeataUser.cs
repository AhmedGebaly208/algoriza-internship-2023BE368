using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VezeataApplication.Core.Enum;

namespace VezeataApplication.Core.Entities
{
    public class VezeataUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Image { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? AccountType { get; set; } // specify if user is admin or doctor or patient

        public virtual ICollection<DiscountCodeCoupon> DiscountCodes { get; set; }
        public virtual List<Booking> Bookings { get; set; }
    }
}
