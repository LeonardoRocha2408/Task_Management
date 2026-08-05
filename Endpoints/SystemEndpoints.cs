using Enums.ServicesDTOs;
using Enums.ServicesEnums.ProjectAndTasks;
using Microsoft.AspNetCore.Mvc;
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

            app.MapPost("project/{ProjectId}/create_task", async (CreateTaskRequest request, [FromRoute] Guid ProjectId, HttpContext context, SystemServices system) =>
            {
                string? userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(userId, out Guid Id))
                {
                    return Results.Unauthorized();
                }

                TaskActionsResult result = await system.CreateTasks(request, Id, ProjectId);

                return result switch
                {
                    TaskActionsResult.UserNotFound => Results.NotFound("User not found"),

                    TaskActionsResult.InvalidTitle => Results.BadRequest("Title is not in the expected format"),

                    TaskActionsResult.TitleTooLong => Results.BadRequest("Title is bigger than expected"),

                    TaskActionsResult.Created => Results.Ok("Task created"),

                    _ => Results.BadRequest("Unknow error")
                };
            })
                .RequireAuthorization();
        }
    }
}
