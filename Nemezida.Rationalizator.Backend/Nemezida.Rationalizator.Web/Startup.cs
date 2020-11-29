namespace Nemezida.Rationalizator.Web
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    using Nemezida.Rationalizator.Web.DataAccess;
    using Nemezida.Rationalizator.Web.Services;
    using Nemezida.Rationalizator.Web.Impl;
    using Nemezida.Rationalizator.Web.Hubs;
    using AutoMapper;
    using AutoMapper.EquivalencyExpression;

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
            services.AddSignalR();

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDbContext<SystemDbContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("Main")));


            services.AddAutoMapper((serviceProvider, automapper) => 
            {
                automapper.AddCollectionMappers();
                automapper.UseEntityFrameworkCoreModel<SystemDbContext>(serviceProvider);
            }, this.GetType().Assembly);

            services.AddSwaggerGen();

            services.AddScoped<IFileStorage, LocalFileStorage>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(x => 
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chathub");
            });
        }
    }
}
