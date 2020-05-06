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
using Carhealth.Models.IdentityModels;
using AspNetCore.Identity.Mongo;
using System.Linq;

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
            /// Before changing the repo type you should do:
            /// 1. Logout from app for clear cookie in your browser.
            /// 2. In Models\IdentityModels\Role.cs AND User.cs change parent class
            /// 3. Uncomit in Models\IdentityModels\UserContext.cs
            /// 4. Unhandled issue with UserContext and IdentityRole
            
            //---------------- EF Core identity repository

           // services.AddTransient<ICarRepository, EFCarRepository>(); // EF Core data repository

           // services.AddDbContext<CarContext>(options =>
           //options.UseSqlServer(Configuration.GetConnectionString("CarsDb"))); // for EF Core data repository

           // services.AddDbContext<UserContext>(options =>
           // options.UseSqlServer(Configuration.GetConnectionString("CarHealthIdentityDb")));

           // services.AddIdentity<User, Role>(options => //валидация пароля 
           // {
           //     options.Password.RequiredLength = 4;   // минимальная длина
           //     options.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
           //     options.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
           //     options.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
           //     options.Password.RequireDigit = false; // требуются ли цифры
           //     options.User.RequireUniqueEmail = true; // уникальный email
           // }).
           // AddEntityFrameworkStores<UserContext>();// устанавливает тип хранилища, которое будет применяться в Identity для хранения 
           //                                         //данных. В качестве типа хранилища здесь указывается класс контекста данных.

            //---------------- MongoDb identity repository---------------------


            services.AddTransient<ICarRepository, MongoCarsRepository>(); // MongoDb data repository

            services.AddIdentityMongoDbProvider<User, Role>(identityOptions =>
            {
                identityOptions.Password.RequiredLength = 4;   // минимальная длина
                identityOptions.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
                identityOptions.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
                identityOptions.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
                identityOptions.Password.RequireDigit = false; // требуются ли цифры
                identityOptions.User.RequireUniqueEmail = true; // уникальный email
            }, mongoIdentityOptions =>
            {
                mongoIdentityOptions.ConnectionString = Configuration.GetConnectionString("MongoDbIdentity");
            });


            services.AddTransient<IRepository<List<CarEntity>>, FileRepository>();

          


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