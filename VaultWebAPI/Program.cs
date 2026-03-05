using VaultWebAPI.Data.Repositories;
using VaultWebAPI.Services;

namespace VaultWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<INodeRepository, NodeRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITreeService, TreeService>();
            builder.Services.AddScoped<IHashService, HashService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
