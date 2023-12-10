using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VezeataApplication.Core.Entities;
using VezeataApplication.Core.Helper;
using VezeataApplication.Core.Inetrfaces;
using VezeataApplication.Core.Interfaces;
using VezeataApplication.Repository;
using VezeataApplication.Repository.Data;
using VezeataApplication.Repository.Repository;
using VezeataApplication.Services.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddDbContext<ApplicationDbContext>
(
    options => options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
    );
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();  // Add additional logging providers if needed
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IDoctorService, DoctorService>();
builder.Services.AddTransient<IPatientService, PatientService>();
builder.Services.AddTransient<IAdminService, AdminService>();
builder.Services.AddTransient<IDiscountCodeService, DiscountCodeService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IUserAccessor, UserAccessor>();
builder.Services.AddTransient<IAppointmentService, AppointmentService>();

builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddIdentity<VezeataUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
//builder.Services.AddAuthorization(options =>
//    options.AddPolicy("PatientPolicy", policy =>
//    policy.RequireRole("Patient"))
//    );
builder.Services.Configure<Jwt>(builder.Configuration.GetSection("JWT"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.RequireHttpsMetadata = false;
    options.SaveToken = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
    };
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var serviceProvider = serviceScope.ServiceProvider;
    DataSeeder.InitializeAdmin(serviceProvider).Wait();
    try
    {
        var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

        // Ensure the database is created
        dbContext.Database.EnsureCreated();

        // Seed the data
        DataSeeder.SeedSpecializationsAsync(dbContext).Wait();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
