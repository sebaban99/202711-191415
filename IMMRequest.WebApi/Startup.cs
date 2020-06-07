using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using IMMRequest.BusinessLogic;
using IMMRequest.DataAccess;
using IMMRequest.BusinessLogic.Interfaces;
using IMMRequest.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using IMMRequest.Domain;
using Type = IMMRequest.Domain.Type;
using System.Diagnostics.CodeAnalysis;

namespace IMMRequest.WebApi
{
    [ExcludeFromCodeCoverage]
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
            services.AddControllers();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //AddScoped Logic
            services.AddScoped<IRequestLogic, RequestLogic>();
            services.AddScoped<ILogLogic, LogLogic>();
            services.AddScoped<ISessionLogic, SessionLogic>();
            services.AddScoped<IAdminLogic, AdminLogic>();
            services.AddScoped<ITypeLogic, TypeLogic>();
        
            //AddScoped Repository
            services.AddScoped<IRequestRepository, RequestRepository>();
            services.AddScoped<IRepository<AFValue>, AFValueRepository>();
            services.AddScoped<ITypeRepository, TypeRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<IRepository<Admin>, AdminRepository>();
            services.AddScoped<IRepository<Topic>, TopicRepository>();
            services.AddScoped<IRepository<AdditionalField>, AdditionalFieldRepository>();
            services.AddScoped<IRepository<AFRangeItem>, RangeRepository>();


            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                    builder.SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddDbContext<DbContext, IMMRequestContext>(
                o => o.UseSqlServer(Configuration.GetConnectionString("IMMRequest")));

            //services.AddCors(
            //    options =>
            //    {
            //        options.AddPolicy(
            //              "CorsPolicy",
            //          builder => builder
            //             .AllowAnyOrigin()
            //             .AllowAnyMethod()
            //             .AllowAnyHeader()
            //             .AllowCredentials()
            //          );
            //    }
            //);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseCors("CorsPolicy");
        }
    }
}
