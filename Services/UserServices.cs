using Enums.UserEnums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Shared.UserDTOs;
using TaskManagementAPI.Entities;

namespace TaskManagementAPI.Services
{
    public class UserServices
    {
        // Define the general class attributes
        private readonly DbContextEntity _context;
        private readonly AuthServices _authServices;


        // Define the class constructor
        public UserServices(DbContextEntity context, AuthServices authServices)
        {
            _context = context;
            _authServices = authServices;
        }

        // Create the account method and see if the current email is already in the database
        public async Task<CreateAccountResult> CreateAccount(CreateAccountRequest dto)
        {
            UserEntity? user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user != null)
            {
                return CreateAccountResult.Email_Already_Exists;
            } 
            else
            {
                var userToAdd = new UserEntity()
                {
                    Id = new Guid(),
                    Role = Role.User,
                    Email = dto.Email,
                    UserName = dto.UserName
                };
                userToAdd.PasswordHash = _authServices.HashPassword(userToAdd, dto.Password);

                await _context.AddAsync(userToAdd);
                await _context.SaveChangesAsync();
            }
            return CreateAccountResult.Created;
        }

        public async Task<(LoginResult, UserEntity? user)> LoginAccount(LoginAccountRequest dto)
        {
            UserEntity? user = await _context.Users
                .FirstOrDefaultAsync(U => U.Email == dto.Email);

            if (user == null)
            {
                return (LoginResult.AccountNotFound, user);
            }
            
            var result = _authServices.VerityPassword(user, user.PasswordHash, dto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return (LoginResult.TheDataIsIncorrect, user);
            }

            return (LoginResult.LoginAccountSuccessfully, user);
        }

        public async Task<ChangePasswordResult> ChangePassword(ChangePasswordRequest dto)
        {
            UserEntity? user = await _context.Users.FirstOrDefaultAsync(user => user.Email == dto.Email);

            if (user == null)
            {
                return ChangePasswordResult.AccountNotFound;
            }

            PasswordVerificationResult result = _authServices.VerityPassword(user, user.PasswordHash, dto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return ChangePasswordResult.WrongData;
            }
            else if (result == PasswordVerificationResult.Success)
            {
                user.PasswordHash = _authServices.HashPassword(user, dto.NewPassword);
                await _context.SaveChangesAsync();
            }

            return ChangePasswordResult.PasswordChagedSuccessfully;
        }

        public async Task<DeleteAccountResult> DeleteAccount(DeleteAccountRequest dto)
        {
            UserEntity? user = await _context.Users.FirstOrDefaultAsync(user => user.Email == dto.Email);

            if (user == null)
            {
                return DeleteAccountResult.AccountNotFound;
            }

            PasswordVerificationResult result = _authServices.VerityPassword(user, user.PasswordHash, dto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return DeleteAccountResult.TheDataIsIncorrect;
            }
            else if (result == PasswordVerificationResult.Success)
            {
                _context.Remove(user);
                await _context.SaveChangesAsync();
            }
            return DeleteAccountResult.DeletedSuccessfully;
        }
    }
}
