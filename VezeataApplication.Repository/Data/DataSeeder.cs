using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VezeataApplication.Core.Entities;

namespace VezeataApplication.Repository.Data

{
    public static class DataSeeder
    {
        public static async Task InitializeAdmin(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<VezeataUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string adminRole = "Admin";
            string adminEmail = "admin@example.com";
            string password = "Admin_123";
            if (await roleManager.FindByNameAsync(adminRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new VezeataUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Ahmed",
                    LastName = "Gebaly",
                };
                var result = await userManager.CreateAsync(adminUser, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
            }
        }
        public static async Task SeedSpecializationsAsync(ApplicationDbContext context)
        {
            // Check if specializations already exist
            if (await context.Specializations.AnyAsync())
            {
                return; // Data already seeded
            }

            // Add specializations
            var specializations = new List<Specialization>
        {
            new Specialization { NameAr = "طب و جراحة العيون", NameEn = "Ophthalmology" },
            new Specialization { NameAr = "طب الاسنان", NameEn = "Dentistry" },
            new Specialization { NameAr = "جراحة القلب و القسطرة", NameEn = "Cardiac surgery and catheterization" },
            new Specialization { NameAr = "الجراحة العامة", NameEn = "General Surgery" },
            new Specialization { NameAr = "جراحة العظام", NameEn = "Orthopaedic Surgery" },
            new Specialization { NameAr = "جراحة الأطفال", NameEn = "Pediatric surgery" },
            new Specialization { NameAr = "جراحة المخ والأعصاب", NameEn = "Neurosurgery" },
            new Specialization { NameAr = "التخدير", NameEn = "Anesthesia" },
            new Specialization { NameAr = "الجهاز الهضمي والكبد", NameEn = "Digestive system and liver" },
            new Specialization { NameAr = "طب الامراض النفسية", NameEn = "Psychiatry" },
            new Specialization { NameAr = "السكري", NameEn = "Diabetes" },
            new Specialization { NameAr = "طب القلب العامة", NameEn = "General cardiology" },
            new Specialization { NameAr = "جراحة المسالك البولية", NameEn = "Urology" },
            new Specialization { NameAr = "جراحة الحروق و التجميل", NameEn = "Burns and plastic surgery" },
            new Specialization { NameAr = "طب أمراض الكلى", NameEn = "Nephrology" },
            new Specialization { NameAr = "أمراض النساء و الولادة", NameEn = "Obstetrics and gynecology" },
            new Specialization { NameAr = "طب الباطني عام", NameEn = "General internal medicine" },
            new Specialization { NameAr = "جراحة الفم والوجه والفكين", NameEn = "Oral and maxillofacial surgery" },
            new Specialization { NameAr = "أمراض الجلدية", NameEn = "Skin diseases" },
            new Specialization { NameAr = "طب الانف والاذن والحنجرة", NameEn = "Otolaryngology" },

        };

            await context.Specializations.AddRangeAsync(specializations);
            await context.SaveChangesAsync();
        }
    }
}
