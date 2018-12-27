using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResourceAllocation.DataLayer.Designers;
using ResourceAllocation.DataLayer.FashionModels;
using ResourceAllocation.DataLayer.Show;
using ResourceAllocation.DataLayer.Shows;
using ResourceAllocation.Services.Designers;
using ResourceAllocation.Services.FashionModels;
using ResourceAllocation.Services.Show;

namespace ResourceAllocation.Api
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
            services.AddCors();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddTransient<IFashionModelsService, FashionModelsService>();
            services.AddTransient<IFashionModelsRepository, FashionModelsRepository>();

            services.AddTransient<IDesignersService, DesignersService>();
            services.AddTransient<IDesignersRepository, DesignersRepository>();

            services.AddTransient<IShowsService, ShowsService>();
            services.AddTransient<IShowsRepository, ShowsRepository>();

            var connection = @"Server=(localdb)\mssqllocaldb;Database=ResourceAllocation;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<ResourceAllocationDbContext>
                (options => options.UseSqlServer(connection));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ResourceAllocationDbContext>();
                context.Database.EnsureCreated();
                context.Database.Migrate();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseCors(builder => builder.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin());
        }
    }
}
