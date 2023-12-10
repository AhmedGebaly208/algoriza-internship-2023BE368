using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VezeataApplication.Core.Entities;
using VezeataApplication.Core.Helper;
using VezeataApplication.Core.Inetrfaces;
using VezeataApplication.Core.Models;

namespace VezeataApplication.Services.Service
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly UserManager<VezeataUser> _userManager;
        private readonly SignInManager<VezeataUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IOptions<Jwt> jwt;
        private readonly Jwt _jwt;

        public AuthService(ILogger<AuthService> logger, UserManager<VezeataUser> userManager, SignInManager<VezeataUser> signInManager, RoleManager<IdentityRole> roleManager, IOptions<Jwt> jwt)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            this.jwt = jwt;
            _jwt = jwt.Value;
        }
        public async Task<AuthModel> RegisterAsync(PateintRegistrationModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) != null)
                return new AuthModel { Message = "Email is already registered" };
            if (await _userManager.FindByNameAsync(model.UserName) != null)
                return new AuthModel { Message = "User name is already registered" };

            var patientRole = await _roleManager.FindByNameAsync("Patient");
            var user = new VezeataUser
            {
                Image = model.Image,
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Phone,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                AccountType = patientRole.Name
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description }, ";
                    _logger.LogError($"Registration error for user {model.Email}: {error.Description}");

                }
                return new AuthModel { Message = errors };
            }
            await _userManager.AddToRoleAsync(user, "Patient");

            var jwtSecurityToken = await CreateJwtToken(user);

            _logger.LogInformation($"User {user.UserName} registered successfully.");

            return new AuthModel
            {
                Email = user.Email,
                ExpireOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Role =  patientRole.Name ,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName = user.UserName
            };
        }

        public async Task<AuthModel> LoginAsync(UserLoginModel model)
        {
            var authModel = new AuthModel();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Email or Password is incorrect!";
                _logger.LogWarning($"Failed login attempt for email {model.Email}");
                return authModel;
            }
            
           // var patientRole = await _roleManager.FindByNameAsync("Patient");
            var jwtSecurityToken = await CreateJwtToken(user);
            var role = await _userManager.GetRolesAsync(user);
            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.UserName = user.UserName;
            authModel.ExpireOn = jwtSecurityToken.ValidTo;
            authModel.Role = role.FirstOrDefault();

            await _signInManager.SignInAsync(user, true);
            
            _logger.LogInformation($"User {user.UserName} logged in successfully.");
            
            return authModel;
        }

        private async Task<JwtSecurityToken> CreateJwtToken(VezeataUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(_jwt.DurationInHours),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
