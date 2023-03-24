using Irontrax.Models;
using Irontrax.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Irontrax.Services.Identity
{
    public class IrontraxUserStore : IUserStore<IrontraxUser>, IUserPasswordStore<IrontraxUser>
    {
        private readonly IUserManageable _userManager;

        public IrontraxUserStore(IUserManageable userManager)
        {
            _userManager = userManager;
        }

        #region Userstore methods
        public async Task<IdentityResult> CreateAsync(IrontraxUser user, CancellationToken cancellationToken)
        {
            await _userManager.CreateUser(user);
            return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync(IrontraxUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public async Task<IrontraxUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await _userManager.FindById(userId);
        }

        public async Task<IrontraxUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return await _userManager.FindByUsername(normalizedUserName);
        }

        public Task<string> GetNormalizedUserNameAsync(IrontraxUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(IrontraxUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(IrontraxUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(IrontraxUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(IrontraxUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(IrontraxUser user, CancellationToken cancellationToken)
        {
            await _userManager.UpdateUser(user);
            return IdentityResult.Success;
        }
        #endregion

        #region Passwordstore methods
        public Task SetPasswordHashAsync(IrontraxUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<bool> HasPasswordAsync(IrontraxUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task<string> GetPasswordHashAsync(IrontraxUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }
        #endregion
    }
}
