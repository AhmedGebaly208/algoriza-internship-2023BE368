using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeataApplication.Core.Enum;

namespace VezeataApplication.Core.Models
{
    public class PateintRegistrationModel : RegistrationModel
    {
        public string? Image { get; set; }
       
    }
}
