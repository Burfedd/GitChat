using GitChat.Server.Hubs;
using Microsoft.AspNetCore.ResponseCompression;
using GitChat.Server.Data;
using Microsoft.EntityFrameworkCore;
using GitChat.Server.Services;
using GitChat.Server.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
            using (var context = builder.Services.BuildServiceProvider().GetService<ApplicationDbContext>())
            {
                context.Database.Migrate();
            }

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["JwtIssuer"],
                        ValidAudience = builder.Configuration["JwtAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSecurityKey"]))
                    };
                });

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddScoped<IChatService, ChatService>();

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
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllers();
            app.MapHub<ChatHub>("/chathub");
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}