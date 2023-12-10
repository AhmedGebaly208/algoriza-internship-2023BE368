using System.ComponentModel.DataAnnotations;

namespace VezeataApplication.Core.Models
{
    public class DoctorRegistrationModel : RegistrationModel
    {
        [Required]
        public string Image { get; set; }

        [Required]
        public string SpecializationName { get; set; }
    }
}
