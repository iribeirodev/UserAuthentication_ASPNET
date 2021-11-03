using Microsoft.Extensions.DependencyInjection;
using ShopAutenticacao.Request;

namespace ShopAutenticacao.Services.Config
{
    public class DbInitializer : IDbInitializer
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DbInitializer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public void SeedData()
        {
            using (var serviceScope = _scopeFactory.CreateScope()) 
            {
                var userService = serviceScope.ServiceProvider.GetService<UserService>();
                if (userService.GetByUsername(username: "administrator") == null)
                {
                    userService.Create(new CreateUserRequest{
                        Username = "administrator",
                        Password = "admin123",
                        Role = "manager"
                    });
                }
            }
        }
    }
}