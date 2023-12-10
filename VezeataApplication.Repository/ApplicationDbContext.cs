using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VezeataApplication.Core.Entities;

namespace VezeataApplication.Repository
{
    public class ApplicationDbContext : IdentityDbContext<VezeataUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        }
        protected override async void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Times>()
                .HasOne(t => t.Appointment)
                .WithMany(a => a.Times)
                .HasForeignKey(t => t.AppointmentId);


            builder.Entity<Doctor>()
                .HasOne(d => d.Specialization)
                .WithMany(s => s.Doctors)
                .HasForeignKey(d => d.SpecializationId);

            builder.Entity<Booking>()
                .HasOne(b => b.DiscountCodeCoupon)
                .WithMany(dc => dc.Bookings)
                .HasForeignKey(b => b.CouponId);

            builder.Entity<VezeataUser>()
                .HasMany(vu => vu.Bookings)
                .WithOne(b => b.User)
                .OnDelete(DeleteBehavior.NoAction);

        }
        public DbSet<VezeataUser> VezeataUsers { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Times> Times { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<DiscountCodeCoupon> DiscountCodeCoupons { get; set; }
    }
}
