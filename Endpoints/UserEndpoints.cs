using Enums.UserEnums;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Shared.UserDTOs;
using TaskManagementAPI.Entities;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Endpoints
{
    public class UserEndpoints : InterfaceEndpoints
    {
        public void MapEndpoints(WebApplication app)
        {
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
                                SameSite = SameSiteMode.Lax,
                                Expires = DateTime.UtcNow.AddDays(7)
                            }
                            );
                        return Results.Ok("Login account successfully");

                    default:
                        return Results.BadRequest("Unknow error");
                        
                        
                }
            })
                .RequireRateLimiting("LimiterLogin");

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
    