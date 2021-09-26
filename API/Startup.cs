using API.Extensions;
using API.Middleware;
using Application.Features.Auth.RequestModels;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API
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
            services.AddDatabaseServices(Configuration);
            services.AddControllers()
                .AddFluentValidation(f =>
                {
                    f.RegisterValidatorsFromAssemblyContaining<LoginCommand>();
                    f.DisableDataAnnotationsValidation = true;
                });
            services.AddControllers();
            services.AddApplicationServices();
            services.AddIdentityServices(Configuration);
            services.AddMediatR(typeof(LoginCommand).Assembly);
            services.AddSwaggerDocumentation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwaggerDocumention();

            }
            
            app.UseMiddleware<ExceptionMiddleware>();   
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseCors("CorsPolicy");
            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}