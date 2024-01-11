using API.Extensions;
using Database;
using System.Text.Json.Serialization;

namespace API
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure(Configuration);

            //services.AddApplication(); DependencyInject from Application

            services.AddHealthChecks();
            services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000")
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });

            services.AddSwagger();

            services.ConfigureIdentity();

            services.ConfigureJWT(Configuration);

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/api/Error");
                app.UseHsts();
            }
            app.UseHealthChecks("/api/health");

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ArtworkProject");
                c.InjectStylesheet("/swagger-ui/SwaggerDark.css");
            });
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

            if (env.IsDevelopment()) app.UseCors("AllowAll");
            app.UseAuthorization();

            app.UseEndpoints(e =>
            {
                e.MapControllers();
                e.MapFallbackToFile("index.html");
            });
        }
    }
}
