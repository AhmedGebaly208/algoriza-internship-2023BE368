namespace VezeataApplication.Core.Entities
{
    public class Specialization
    {
        public int Id { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public virtual ICollection<Doctor> Doctors { get; set; }
    }
}
