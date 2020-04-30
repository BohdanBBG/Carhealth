using Carhealth.Models; // пространство имен контекста данных UserContext
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Carhealth.Repositories;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace Carhealth
{
    public class Startup
    {

        IWebHostEnvironment _env;
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            _env = env;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CarContext>(options =>
          options.UseSqlServer(Configuration.GetConnectionString("CarsDb")));

            services.AddTransient<IRepository<List<CarEntity>>, FileRepository>();

            //services.AddTransient<ICarRepository, EFCarRepository>(); // EF Core data repository
            services.AddTransient<ICarRepository, MongoCarsRepository>(); // MongoDb data repository

            services.AddDbContext<UserContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("CarHealthIdentityDb")));

            services.AddIdentity<User, IdentityRole>(options => //валидация пароля 
            {
                options.Password.RequiredLength = 4;   // минимальная длина
                options.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
                options.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
                options.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
                options.Password.RequireDigit = false; // требуются ли цифры
                options.User.RequireUniqueEmail = true; // уникальный email
            }).
            AddEntityFrameworkStores<UserContext>();// устанавливает тип хранилища, которое будет применяться в Identity для хранения 
                                                    //данных. В качестве типа хранилища здесь указывается класс контекста данных.


            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app)
        {
            
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(routes =>
            {
                routes.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");
            });
        }
    }
}