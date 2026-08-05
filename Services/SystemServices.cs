using Enums.ServicesDTOs;
using Enums.ServicesEnums.ProjectAndTasks;
using Microsoft.EntityFrameworkCore;
using Shared.ServicesDTOs;
using TaskManagementAPI.Entities;

namespace TaskManagementAPI.Services
{
    public class SystemServices
    {
        public readonly DbContextEntity _context;

        public SystemServices(DbContextEntity context)
        {
            _context = context;
        }

        // Create a new project, checking if users id exists and title lenght
        public async Task<ProjectActionsResult> CreateProject(CreateProjectRequest dto, Guid Id)
        {
            UserEntity? user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == Id);

            if (user == null) 
            {
                return ProjectActionsResult.UserNotFound;
            }

            if (dto.Title.Length < 1)
            {
                return ProjectActionsResult.InvalidTitle;
            }
            else if (dto.Title.Length > 100)
            {
                return ProjectActionsResult.TitleTooLong;
            }

            var project = new ProjectEntity
            {
                Id = new Guid(),
                OwnerId = user.Id,
                Title = dto.Title,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow
            };

            await _context.AddAsync(project);
            await _context.SaveChangesAsync();

            return ProjectActionsResult.Created;
        }

        public async Task<TaskActionsResult> CreateTasks(CreateTaskRequest dto, Guid Id, Guid ProjectId)
        {
            UserEntity? user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == Id);
            
            Guid assignedUserId = await _context.Users
                .Where(u => u.Email == dto.AssignedUser)
                .Select(u => u.Id)
                .FirstOrDefaultAsync();

            if (user == null) 
            {
                return TaskActionsResult.UserNotFound;
            }
            if (dto.Title.Length < 1)
            {
                return TaskActionsResult.InvalidTitle;
            }
            else if (dto.Title.Length > 100)
            {
                return TaskActionsResult.TitleTooLong;
            }

            var task = new TaskEntity()
            {
                Id = new Guid(),
                ProjectId = ProjectId,
                Title = dto.Title,
                Description = dto.Description,
                Creator = Id,
                AssignedUser = assignedUserId,
                DueDate = dto.DueDate,
                CreatedAt = DateTime.UtcNow
            };

            await _context.AddAsync(task);
            await _context.SaveChangesAsync();

            return TaskActionsResult.Created;
        }
    }
}
