using VezeataApplication.Core.Models;

namespace VezeataApplication.Core.Inetrfaces
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(PateintRegistrationModel model);
        Task<AuthModel> LoginAsync(UserLoginModel model);
       // Task<string> AddRoleAsync(AddRoleModel model);
    }
}
