using Enums.ServicesEnums;
using Shared.ServicesDTOs;
using System.Security.Claims;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Endpoints
{
    public class SystemEndpoints : InterfaceEndpoints
    {
        public void MapEndpoints(WebApplication app)
        {
            app.MapPost("create_project", async (CreateProjectRequest request, HttpContext context, SystemServices system) =>
            {
                string? userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!Guid.TryParse(userId, out Guid Id))
                {
                    return Results.Unauthorized();
                }

                ProjectActionsResult result = await system.CreateProject(request, Id);

                return result switch
                {
                    ProjectActionsResult.TitleTooLong => Results.BadRequest("Title is bigger than allowed"),

                    ProjectActionsResult.InvalidTitle => Results.BadRequest("Title not match with expected format"),

                    ProjectActionsResult.UserNotFound => Results.Unauthorized(),

                    ProjectActionsResult.Created => Results.Created(),

                    _ => Results.BadRequest("Unknow error")
                };
            })
                .RequireAuthorization()
                .RequireRateLimiting("LimiterCreateProjects");
        }
    }
}
