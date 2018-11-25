using Lotto.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Lotto.Services.AuthService;
using Microsoft.AspNetCore.Http;
using Lotto.DAL.TransactionRepository;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Storage;
using Lotto.Api.Hubs;
using Lotto.Services.GameHub.GameConnectionStore;
using Lotto.Services.GameHub.GameConnectionHandler;
using Lotto.Services.RollService;
using Lotto.Api.Jobs;

namespace Lotto.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IGameConnectionStore, GameConnectionStore>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IGameConnectionHandler, GameConnectionHandler>();
            services.AddTransient<IRollService, RollService>();

            services.AddCors();

            var connection = Configuration.GetConnectionString("defaultConnection");
            services.AddDbContext<DataContext>(options => options.UseSqlServer(connection));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
               {
                   options.TokenValidationParameters = TokenHelper.GetTokenValidationParameters();

                   //signalr
                   options.Events = new JwtBearerEvents
                   {
                       OnMessageReceived = context =>
                       {
                           var signalRTokenHeader = context.Request.Query["signalRTokenHeader"];

                           if (!string.IsNullOrEmpty(signalRTokenHeader) &&
                               (context.HttpContext.WebSockets.IsWebSocketRequest || context.Request.Headers["Accept"] == "text/event-stream"))
                           {
                               context.Token = context.Request.Query["signalRTokenHeader"];
                           }
                           return Task.CompletedTask;
                       }
                   };
               });

            services.AddSignalR();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Hackyeah Lotto API", Version = "v1" });

                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "Lotto.API.xml");
                c.IncludeXmlComments(xmlPath);
            });

            services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(connection);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, DataContext context)
        {
            app.UseDeveloperExceptionPage();

            DBInitializer.InitializeDb(context);

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"CustomSwaggerExtensions")),
                RequestPath = new PathString("/SwaggerFiles")
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hackyeah Lotto API");
                c.InjectOnCompleteJavaScript("/SwaggerFiles/customSwaggerAuthFields.js");
            });

            app.UseCors(options =>
            {
                options.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<GameHub>("/game/hub");
            });

            app.UseHangfireDashboard("/hangfire");
            app.UseHangfireServer(); 

            // app.UseHttpsRedirection();

            app.UseMvc();
            app.UseHangfireServer();
            app.UseHangfireDashboard();

            // JOBS
            using (var connection = JobStorage.Current.GetConnection())
            {
                foreach (var job in connection.GetRecurringJobs())
                {
                    RecurringJob.RemoveIfExists(job.Id);
                }
            }

            RecurringJob.AddOrUpdate<RollJob>("RollJob", x => x.Execute(), Cron.Minutely);
            // !JOBS
        }
    }
}
