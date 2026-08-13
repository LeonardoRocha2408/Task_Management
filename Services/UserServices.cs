using Enums.UserEnums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Shared.UserDTOs;
using TaskManagementAPI.Entities;

namespace TaskManagementAPI.Services
{
    public sealed class UserServices
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

        // Create the account method and see if the current email is already in the database and verify password
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
                var result = _authServices.ValidatePassword(dto.Password);
                if (result != Enums.AuthPasswordResult.PasswordIsInTheCorrectFormat)
                {
                    return CreateAccountResult.PasswordIsInTheIncorrectFormat;
                }
                
                var userToAdd = new UserEntity()
                {
                    Id = new Guid(),
                    Role = Role.User,
                    Email = dto.Email,
                    UserName = dto.UserName,
                    CreatedAt = DateTime.UtcNow
                };
                userToAdd.PasswordHash = _authServices.HashPassword(userToAdd, dto.Password);

                await _context.AddAsync(userToAdd);
                await _context.SaveChangesAsync();
            }
            return CreateAccountResult.Created;
        }


        // Log in and return a corresponding enum about the result
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


        // Get user profile and return some data like Email, UserName and path profile stored in MySQL database
        public async Task<MeResponse?> GetUser(Guid Id)
        {
            UserEntity? user = await _context.Users
                .FirstOrDefaultAsync(U => U.Id == Id);;

            MeResponse? userResponse = new();
            if (user is null)
            {
                return null;
            }

            return new MeResponse()
            {
                Email = user.Email,
                UserName = user.UserName,
                PathProfilePicture = user.ProfilePicture,
            };
        }


        // Validates if the image or folder already exists, stores the image path in database and return path to frontend
        public async Task<string> UploadProfilePicture(IFormFile image, Guid Id)
        {
            var fileName = $"{Id}{Path.GetExtension(image.FileName)}";

            var folder = Path.Combine(
                "wwwroot",
                "uploads",
                "profiles");

            foreach (var extension in new[] { ".png", ".jpg", ".jpeg", ".webp" })
            {
                var oldFile = Path.Combine(folder, $"{Id}{extension}");

                if (File.Exists(oldFile)) 
                {
                    File.Delete(oldFile);
                }
            }

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var path = Path.Combine(folder, fileName);
            
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using var stream = File.Create(path);
            await image.CopyToAsync(stream);

            await _context.Users
                .Where(u => u.Id == Id)
                .ExecuteUpdateAsync(setters => setters.SetProperty(
                    u => u.ProfilePicture, path));

            return $"/uploads/profiles/{fileName}";
        }

        // Update the password if user forget your password
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
                var resultValidateNewPassword = _authServices.ValidatePassword(dto.NewPassword);
                if (resultValidateNewPassword != Enums.AuthPasswordResult.PasswordIsInTheCorrectFormat)
                {
                    return ChangePasswordResult.NewPasswordIsInTheIncorrectFormat;
                }

                user.PasswordHash = _authServices.HashPassword(user, dto.NewPassword);
                await _context.SaveChangesAsync();
            }

            return ChangePasswordResult.PasswordChagedSuccessfully;
        }


        // Verify if user account exists and delete 
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
