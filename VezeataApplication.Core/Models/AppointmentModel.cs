using System.ComponentModel.DataAnnotations;
using VezeataApplication.Core.Enum;

namespace VezeataApplication.Core.Models
{
    public class AppointmentModel
    {
        [Required]
        public decimal Price { get; set; }
        [Required]
        public List<DayWithTimes> Days { get; set; }
    }
    public class DayWithTimes
    {
        [Required]
        public Days Days { get; set; }
        [Required]
        public List<TimeModel> Times { get; set; }
    }
    public class TimeModel
    {
        [Required]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }
    }

}
