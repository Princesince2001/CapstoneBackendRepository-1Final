using LXP.Core.IServices;
using LXP.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Core.Services
{
    //public class PasswordHistoryService : IPasswordHistoryService
    //{
    //    private readonly IPasswordHistoryRepository _passwordHistoryRepository;

    //    public PasswordHistoryService(IPasswordHistoryRepository passwordHistoryRepository)
    //    {
    //        _passwordHistoryRepository = passwordHistoryRepository;
    //    }

    //    public async Task<bool> UpdatePassword(Guid learnerId, string oldPassword, string newPassword)
    //    {
    //        var passwordHistory = await _passwordHistoryRepository.GetPasswordHistory(learnerId);

    //        if (passwordHistory.OldPassword != oldPassword)
    //        {
    //            return false;
    //        }

    //        passwordHistory.OldPassword = newPassword;
    //        await _passwordHistoryRepository.UpdatePasswordHistory(passwordHistory);

    //        return true;
    //    }
    //}








    public class PasswordHistoryService : IPasswordHistoryService
    {
        private readonly IPasswordHistoryRepository _passwordHistoryRepository;

        public PasswordHistoryService(IPasswordHistoryRepository passwordHistoryRepository)
        {
            _passwordHistoryRepository = passwordHistoryRepository;
        }

        public async Task<bool> UpdatePassword(string learnerId, string oldPassword, string newPassword)
        {
            var passwordHistory = await _passwordHistoryRepository.GetPasswordHistory(Guid.Parse(learnerId));

            // Hashing the old password provided by the learner.
            var oldPasswordHash = HashPassword(oldPassword);

            if (passwordHistory.NewPassword != oldPasswordHash)
            {
                return false;
            }

            passwordHistory.OldPassword = passwordHistory.NewPassword;

            // Hashing the new password provided by the learner.
            var newPasswordHash = HashPassword(newPassword);

            passwordHistory.NewPassword = newPasswordHash;
            await _passwordHistoryRepository.UpdatePasswordHistory(passwordHistory);

            return true;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }


        private string SomeHashFunction(string password)
        {
            throw new NotImplementedException();
        }
    }






}
