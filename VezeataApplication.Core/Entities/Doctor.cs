namespace VezeataApplication.Core.Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceAfterDiscount { get; set; }
        public virtual VezeataUser User { get; set; }
        public int SpecializationId { get; set; }
        public virtual Specialization Specialization { get; set; }
        public virtual List<Appointment> Appointments { get; set; }
    }
}
