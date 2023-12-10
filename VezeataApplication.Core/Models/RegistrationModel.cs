using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VezeataApplication.Core.Enum;

namespace VezeataApplication.Core.Models
{
    public class RegistrationModel
    {
        [Required]
        [MinLength(2)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(2)]
        public string LastName { get; set; }
        [Required]
        [MinLength(2)]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MaxLength(11)]
        public string Phone { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
