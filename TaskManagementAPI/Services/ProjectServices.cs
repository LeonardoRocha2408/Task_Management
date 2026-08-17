using Enums.ServicesEnums.ProjectAndTasks;
using Microsoft.EntityFrameworkCore;
using Shared.ServicesDTOs;
using TaskManagementAPI.Entities;

namespace TaskManagementAPI.Services
{
    public sealed class ProjectServices
    {
        public readonly DbContextEntity _context;

        public ProjectServices(DbContextEntity context)
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

        public async Task<List<ResponseProjects>> GetProjects(Guid Id)
        {
            return await _context.Projects
                .AsNoTracking()
                .Where(p => p.OwnerId == Id)
                .Select(p => new ResponseProjects
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    CreatedAt = p.CreatedAt,
                    Role = "Owner"
                }).ToListAsync();
        }

        public async Task<List<ResponseProjects>> GetParticipingProjects(Guid Id)
        {
            return await _context.ProjectsMembers
                .AsNoTracking()
                .Where(pm => pm.UserId == Id)
                .Select(pm => new ResponseProjects
                {
                    Id = pm.ProjectId,
                    Title = pm.Project.Title,
                    Description = pm.Project.Description,
                    Role = pm.Role.ToString()
                }).ToListAsync();
        }
    }
}
