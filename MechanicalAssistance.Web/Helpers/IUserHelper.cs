using MechanicalAssistance.Web.Data.Entities;
using MechanicalAssistance.Web.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace MechanicalAssistance.Web.Helpers
{
    public interface IUserHelper
    {

        Task<UserEntity> GetUserByEmailAsync(string email);

        Task<UserEntity> GetUserAsync(Guid userId);

        Task CheckRoleAsync(string roleName);

        Task<IdentityResult> AddUserAsync(UserEntity user, string password);
        Task AddUserToRoleAsync(UserEntity user, string roleName);
        Task<bool> IsUserInRoleAsync(UserEntity user, string roleName);
        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task LogoutAsync();

        Task<string> GenerateEmailConfirmationTokenAsync(UserEntity user);

        Task<IdentityResult> UpdateUserAsync(UserEntity user);

        Task<string> GeneratePasswordResetTokenAsync(UserEntity user);
        Task<IdentityResult> ChangePasswordAsync(UserEntity user, string oldPassword, string newPassword);
        Task<IdentityResult> ConfirmEmailAsync(UserEntity user, string token);
        Task<SignInResult> ValidatePasswordAsync(UserEntity user, string password);
        Task<IdentityResult> ResetPasswordAsync(UserEntity user, string token, string password);
    }
}
