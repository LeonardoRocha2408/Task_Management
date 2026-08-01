namespace TaskManagementAPI.Endpoints
{
    public static class EndpointExtensions
    {
        public static void MapEndpoints(this WebApplication app)
        {
            new UserEndpoints().MapEndpoints(app);
        }
    }
}
