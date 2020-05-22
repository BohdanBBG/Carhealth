using CarHealth.Api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CarHealth.Api.Repositories;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using AspNetCore.Identity.Mongo;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using CarHealth.Api.Models.IdentityModels;

namespace CarHealth.Api
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
            var config = Configuration.Get<ApplicationSettings>();

            services.Configure<ApplicationSettings>(Configuration);

            ConfigureMongoDb(services, config);
            //ConfigureEFCoreDb(services, config);

            services.AddTransient<IRepository<List<CarEntity>>, FileRepository>();

            services.AddControllersWithViews();

            services.AddLogging();

            services.AddMvc();

            services.AddCors(options =>
            {
                options.AddPolicy("default", builder =>
                {
                    builder.AllowAnyOrigin();
                     // .WithOrigins(config.Cors.AllowedOrigins.ToArray())
                     // .AllowAnyMethod()
                     // .AllowAnyHeader()
                     // .AllowCredentials();
                });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CarHealth", Version = "v1" });
            });
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

            app.UseCors("default");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();



            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CarHealth V1");
            });

            app.UseEndpoints(routes =>
            {
                routes.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");
            });
        }

        private void ConfigureMongoDb(IServiceCollection services, ApplicationSettings config)
        {

            services.AddTransient<MongoClient>(sp =>
            {
                return new MongoClient(config.MongoDb.ConnectionString);
            });


            services.AddTransient<ICarRepository, MongoRepository>(sp =>
            {
                var mongoClient = sp.GetService<MongoClient>();

                return new MongoRepository(mongoClient, config.MongoDb.MainDb);
            }); // MongoDb data repository


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
                mongoIdentityOptions.ConnectionString = config.MongoDb.MongoDbIdentity;
            });
        }

        private void ConfigureEFCoreDb(IServiceCollection services, ApplicationSettings config)
        {
            /// Before changing the repo type you should do:
            /// 1. Logout from app for clear cookie in your browser.
            /// 2. In Models\IdentityModels\Role.cs AND User.cs change parent class
            /// 3. Uncomit in Models\IdentityModels\UserContext.cs
            /// 4. Everywhere change Role to IdentityRole
            /// 5. Unhandled issue with UserContext and IdentityRole


           // services.AddTransient<ICarRepository, EFCarRepository>(); // EF Core data repository

           // services.AddDbContext<CarContext>(options =>
           //options.UseSqlServer(config.EFCoreDb.CarsDb)); // for EF Core data repository

           // services.AddDbContext<UserContext>(options =>
           //options.UseSqlServer(config.EFCoreDb.CarHealth.ApiIdentityDb));

           // services.AddIdentity<User, IdentityRole>(options => //валидация пароля 
           // {
           //     options.Password.RequiredLength = 4;   // минимальная длина
           //     options.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
           //     options.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
           //     options.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
           //     options.Password.RequireDigit = false; // требуются ли цифры
           //     options.User.RequireUniqueEmail = true; // уникальный email
           // })
           // .AddEntityFrameworkStores<UserContext>();// устанавливает тип хранилища, которое будет применяться в Identity для хранения 
           //                                                 // данных.В качестве типа хранилища здесь указывается класс контекста данных.

        }

    }
}