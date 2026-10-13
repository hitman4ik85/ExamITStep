using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GalleryApi.Data;
namespace GalleryApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<GalleryApiContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("GalleryApiContext") ?? throw new InvalidOperationException("Connection string 'GalleryApiContext' not found.")));

            // Add services to the container.

            builder.Services.AddControllers();

            var myPermissions = "MyPermissionsForReactNative";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(myPermissions,
                (policy) =>
                {
                    policy.AllowAnyOrigin().
                        AllowAnyHeader().
                        AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();

            app.UseCors(myPermissions);

            app.MapControllers();

            app.Run();
        }
    }
}
