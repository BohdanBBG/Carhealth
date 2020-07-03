using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CarHealth.Api.Repositories;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using CarHealth.Api.Helpers;
using Microsoft.Extensions.Logging;
using CarHealth.Api.Contexts;
using CarHealth.Api.Repositories.EFCoreRepository;

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


            services.AddControllers();

            services.AddLogging(configure => configure.AddConsole())
                 .AddTransient<Program>();

            services.AddMvcCore();

            //добавляем авторизацию, благодаря этому будут работать атрибуты Authorize
            services.AddAuthorization(options =>
               // политики позволяют не работать с Roles magic strings, содержащими перечисления ролей через запятую
               options.AddPolicy("AdminsOnly", policyUser =>
               {
                   policyUser.RequireClaim("role", "admin");
               })
           );
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                                           JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme =
                                           JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.Authority = config.JwtBearerAuth.Authority;
                o.Audience = config.JwtBearerAuth.Audience;
                o.RequireHttpsMetadata = false;
            });

            services.AddCors(options =>
            {
                if (config.Cors != null && config.Cors.AllowedOrigins != null)
                {
                    // задаём политику CORS, чтобы наше клиентское приложение могло отправить запрос на сервер API
                    options.AddPolicy("default", policy =>
                    {
                        policy.WithOrigins(config.Cors.AllowedOrigins.ToArray())
                       .AllowAnyHeader()
                       .AllowAnyMethod();
                    });
                }
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CarHealth.Api", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app)
        {
           
            if (HostingEnvironmentHelper.IsDevelopmentAny())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
               // app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseCors("default");

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CarHealth.Api V1");
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


        }

        private void ConfigureEFCoreDb(IServiceCollection services, ApplicationSettings config)
        {

            services.AddTransient<ICarRepository, EFCarRepository>(); // EF Core data repository

            services.AddDbContext<CarContext>(options =>
           options.UseSqlServer(config.EFCoreDb.CarsDb)); // for EF Core data repository
         
        }

    }
}