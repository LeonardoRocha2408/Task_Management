using Enums.UserEnums;
using Microsoft.AspNetCore.Mvc;
using Shared.UserDTOs;
using System.Security.Claims;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Endpoints
{
    public sealed class UserEndpoints : InterfaceEndpoints
    {
        public void MapEndpoints(WebApplication app)
        {
            // Create the account method and see if the current email is already in the database and verify password
            app.MapPost("/create_account", async (CreateAccountRequest request, UserServices user) =>
            {
                var result = await user.CreateAccount(request);

                return result switch
                {
                    CreateAccountResult.Email_Already_Exists => Results.Conflict("Email already exists"),

                    CreateAccountResult.Created => Results.Created(),

                    _ => Results.BadRequest("Unknow error")
                };
            })
                .RequireRateLimiting("LimiterCreateAccount");

            // Log in and return a corresponding enum about the result
            app.MapPost("/login_account", async (LoginAccountRequest request, AuthServices authServices, UserServices user, HttpContext context) =>
            {
                var result = await user.LoginAccount(request);

                switch (result.Item1)
                {
                    case LoginResult.AccountNotFound:
                        return Results.NotFound("Account not found");

                    case LoginResult.TheDataIsIncorrect:
                        return Results.Unauthorized();

                    case LoginResult.LoginAccountSuccessfully:
                        string token = authServices.GenerateToken(result.user!);
 
                        context.Response.Cookies.Append(
                            "accessToken",
                            token,
                            new CookieOptions
                            {
                                HttpOnly = true,
                                Secure = true,
                                SameSite = SameSiteMode.None,
                                Expires = DateTime.UtcNow.AddDays(7)
                            }
                            );
                        return Results.Ok("Login account successfully");

                    default:
                        return Results.BadRequest("Unknow error");
                        
                        
                }
            })
                .RequireRateLimiting("LimiterLogin");

            app.MapPost("/logout", (HttpContext context) =>
            {
                context.Response.Cookies.Delete("accessToken", new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });

                return Results.Ok();
            });

            // Get user profile and return some data like Email, UserName and path profile stored in MySQL database
            app.MapGet("/me", async (HttpContext context, UserServices user) =>
            {
                string? userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!Guid.TryParse(userId, out Guid Id))
                {
                    return Results.Unauthorized();
                }

                MeResponse? userResponse = await user.GetUser(Id);

                if (userResponse is null)
                {
                    return Results.NotFound("Account not found");
                }

                return Results.Ok(userResponse);
            })
                .RequireAuthorization();


            // Validates if the image or folder already exists, stores the image path in database and return path to frontend
            app.MapPost("/upload_profile_picture", async (IFormFile image, HttpContext context, UserServices user) =>
            {
                string[] allowedTypes =
                {
                    "image/png",
                    "image/jpg",
                    "image/jpeg",
                    "image/webp"
                };

                if (!allowedTypes.Contains(image.ContentType))
                {
                    return Results.BadRequest(new
                    {
                        message = "Invalid image"
                    });
                }

                const long maxSize = 5 * 1024 * 1024;
                if (image.Length > maxSize)
                {
                    return Results.BadRequest(new
                    {
                        message = "Invalid image"
                    });
                }
                else if (image.Length == 0)
                {
                    return Results.BadRequest(new
                    {
                        message = "Invalid image"
                    });
                }

                string? userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(userId, out var Id))
                {
                    return Results.Unauthorized();
                }

                string pathImage = await user.UploadProfilePicture(image, Id);

                return Results.Ok(pathImage);
            })
                .DisableAntiforgery()
                .RequireAuthorization();

            // Update the password if user forget your password
            app.MapPatch("/change_password", async ([FromBody] ChangePasswordRequest request, UserServices user) =>
            {
                ChangePasswordResult result = await user.ChangePassword(request);

                return result switch
                {
                    ChangePasswordResult.AccountNotFound => Results.NotFound("Account not exists"),

                    ChangePasswordResult.WrongData => Results.Unauthorized(),

                    ChangePasswordResult.NewPasswordIsInTheIncorrectFormat => Results.BadRequest("New password doesn't match the expected format"),

                    ChangePasswordResult.PasswordChagedSuccessfully => Results.Accepted("Password changed successfully"),

                    _ => Results.BadRequest("Unknown error")
                };
            })
               .RequireRateLimiting("LimiterChangePassword");

            // Delete the user account 
            app.MapDelete("/user/delete_account", async ([FromBody] DeleteAccountRequest request, UserServices user) =>
            {
                DeleteAccountResult result = await user.DeleteAccount(request);

                return result switch
                {
                    DeleteAccountResult.AccountNotFound => Results.NotFound("AccountNotFound"),

                    DeleteAccountResult.TheDataIsIncorrect => Results.Unauthorized(),

                    DeleteAccountResult.DeletedSuccessfully => Results.Accepted("Your account has deleted successfully"),

                    _ => Results.BadRequest("Unknown error")
                };
            }).RequireAuthorization();
        }
    }
}
    