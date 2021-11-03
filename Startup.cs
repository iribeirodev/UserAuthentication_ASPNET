using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShopAutenticacao.Models;
using Microsoft.Extensions.Options;
using ShopAutenticacao.Services;
using ShopAutenticacao.Services.Config;

namespace ShopAutenticacao
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ShopAuthenticationDatabaseSettings>(
                Configuration.GetSection(nameof(ShopAuthenticationDatabaseSettings))
            );
            // A interface IShopAuthenticationDatabaseSettings é registrada em DI com um tempo de vida de serviço singleton.
            // Quando injetada, a instância da interface é resolvida para um objeto ShopAuthenticationDatabaseSettings. 
            services.AddSingleton<IShopAuthenticationDatabaseSettings>(
                c => c.GetRequiredService<IOptions<ShopAuthenticationDatabaseSettings>>().Value
            );

            services.AddSingleton<UserService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDbInitializer, DbInitializer>();

            services.AddControllers();

            // Informando que está sendo utilizada autenticação JWT
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            services.AddAuthentication(c => {
                c.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                c.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(c => {
                c.RequireHttpsMetadata = false;
                c.SaveToken = true;
                c.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ShopAutenticacao", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShopAutenticacao v1"));

                SeedData(app);
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseCors(c => c.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Initialize database with admin data
        /// </summary>
        /// <param name="app"></param>
        private void SeedData(IApplicationBuilder app) 
        {
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope()) 
            {
                var dbInitializer = scope.ServiceProvider.GetService<IDbInitializer>();
                dbInitializer.SeedData();
            }
        }
    }
}
