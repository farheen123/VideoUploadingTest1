using Microsoft.AspNetCore.Http.Features;
using VideoUploadingTest.Models;
using VideoUploadingTest.Services;
using VideoUploadingTest.Services.Interface;

namespace VideoUploadingTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton<IFileService>(sp => new FileService(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ServerMediaFiles")));


            builder.Services.AddControllersWithViews();

            // Configure Kestrel server options
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 200 * 1920 * 1920; // 200 MB
            });

            // Configure form options for handling file uploads
            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 200 * 1920 * 1920; // 200 MB
            });

            // Configure the HTTP request pipeline
            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}