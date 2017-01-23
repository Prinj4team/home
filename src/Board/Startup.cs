using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Board.Data;
using Board.Models;
using Board.Services;
using Microsoft.AspNetCore.Identity;

namespace Board
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();

                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(opts => {
                opts.Password.RequiredLength = 5;   // минимальная длина
                opts.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
                opts.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
                opts.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
                opts.Password.RequireDigit = false; // требуются ли цифры
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Posts}/{action=Index}/{id?}");
            });

            // Создаем Service Scope для инициализации всех сервисов

            using (var serviceScope = app.ApplicationServices

            .GetRequiredService<IServiceScopeFactory>()

            .CreateScope())

            {

                // Получаем экземпляр ApplcationDbContext из ServiceProvider-а

                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                // Применяем непримененные миграции

                context.Database.Migrate();

                // Получаем RoleManager

                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>

                ();

                // Проверяем, есть ли роль Admins. Если нет - добавляем.

                var admins = roleManager.FindByNameAsync("Admins").Result;

                if (admins == null)

                {

                    var roleResult = roleManager.CreateAsync(new IdentityRole("Admins")).Result;

                }

                var boards = roleManager.FindByNameAsync("Boards").Result;

                if (boards == null)

                {

                    var roleResult = roleManager.CreateAsync(new IdentityRole("Boards")).Result;

                }

                // Получаем UserManager

                var userManager =

                serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

                // Проверяем, есть ли пользователь

                var admin = userManager.FindByNameAsync("admin@admin.com").Result;

                if (admin == null)

                {

                    // Если нет - создаем

                    var userResult = userManager.CreateAsync(new ApplicationUser

                    {

                        UserName = "admin@admin.com",

                        Email = "admin@admin.com"

                    }, "Admin123!").Result;

                    admin = userManager.FindByNameAsync("admin@admin.com").Result;

                    // И добавляем ему роль Admins

                    userManager.AddToRoleAsync(admin, "Admins").Wait();

                }

            }
        }
    }
}
