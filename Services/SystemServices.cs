using Enums.ServicesEnums;
using Microsoft.EntityFrameworkCore;
using Shared.ServicesDTOs;
using Shared.UserDTOs;
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
    }
}
