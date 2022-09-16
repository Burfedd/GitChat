using GitChat.Server.Hubs;
using Microsoft.AspNetCore.ResponseCompression;
using GitChat.Server.Data;
using Microsoft.EntityFrameworkCore;
using GitChat.Server.Repository;

namespace GitChat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
            // Add services to the container.


            builder.Services.AddSignalR();
            builder.Services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

            //TODO: Remove
            //builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(ConfigurationExtensions.GetConnectionString(builder.Configuration, "DefaultConnection")));

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
            
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            app.UseResponseCompression();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();


            app.MapRazorPages();
            app.MapControllers();
            app.MapHub<ChatHub>("/chathub");
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}