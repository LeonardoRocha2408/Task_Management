using Enums.ServicesDTOs;
using Enums.ServicesEnums.ProjectAndTasks;
using Microsoft.EntityFrameworkCore;
using Shared.ServicesDTOs;
using TaskManagementAPI.Entities;

namespace TaskManagementAPI.Services
{
    public sealed class TasksServices
    {
        public readonly DbContextEntity _context;

        public TasksServices(DbContextEntity context)
        {
            _context = context;
        }

        // Create a new task, checking if users id exists and title lenght
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

        public async Task<List<ResponseTasks>> GetTasks(Guid Id)
        {
            return await _context.Tasks
                .AsNoTracking()
                .Where(t => t.ProjectId == Id)
                .Select(t => new ResponseTasks()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    AssignedUser = t.AssignedUser,
                    CreatedAt = t.CreatedAt
                }).ToListAsync();
        }
    }
}
