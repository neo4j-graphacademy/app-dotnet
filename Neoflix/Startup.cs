using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Neoflix
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // add api controllers
            services.AddControllers();
            // add jwt authorization/authentication
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtHelper.ConfigureJwt);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            // add static file serving for vue app
            app.UseFileServer();
            app.UseRouting();
            // add auth 
            app.UseAuthentication();
            app.UseAuthorization();
            // register controllers
            app.UseEndpoints(builder => builder.MapControllers());
        }
    }
}