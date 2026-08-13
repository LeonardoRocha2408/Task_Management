using Enums.ServicesDTOs;
using Enums.ServicesEnums.ProjectAndTasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Endpoints
{
    public class TaskEndpoints
    {
        public void MapEndpoints(WebApplication app)
        {
            app.MapPost("project/{ProjectId}/create_task", async (CreateTaskRequest request, [FromRoute] Guid ProjectId, HttpContext context, TasksServices system) =>
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
