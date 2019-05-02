using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ResourceAllocation.DataLayer;
using ResourceAllocation.DataLayer.Designers;
using ResourceAllocation.DataLayer.Artists;
using ResourceAllocation.Services.Designers;
using ResourceAllocation.Services.Artists;
using ResourceAllocation.Services.ResourceAllocation;

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
            services.AddCors(options =>
            {
                options.AddPolicy("LocalCorsConfig", policy => policy.WithOrigins("http://localhost:4200"));
            });

            services.AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddTransient<IArtistsService, ArtistsService>();
            services.AddTransient<IArtistsRepository, ArtistsRepository>();

            services.AddTransient<IDesignersService, DesignersService>();
            services.AddTransient<IDesignersRepository, DesignersRepository>();

            services.AddTransient<IAdjustedWinnerAllocationService, AdjustedWinnerAllocationService>();
            services.AddTransient<IDescendingDemandAllocationService, DescendingDemandAllocationService>();

            var connection = @"Server=(localdb)\mssqllocaldb;Database=ModelLinK;Trusted_Connection=True;ConnectRetryCount=0";
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
                //context.Database.Migrate();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseCors("LocalCorsConfig");
        }
    }
}
