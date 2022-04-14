using Infrastructure.Repositories.Users;
using Microsoft.Extensions.DependencyInjection;
using Service.Users;

namespace User_Api.Configurations
{
    public static class DependencyResolver
    {
        public static void AddServiceResolvers(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}
