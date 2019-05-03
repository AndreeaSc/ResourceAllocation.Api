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
using ResourceAllocation.Api.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace ResourceAllocation.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            configuration = builder.Build();

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

            var connectionStrings = new ConnectionStrings();
            Configuration.GetSection("ConnectionStrings").Bind(connectionStrings);

            services.AddDbContext<ResourceAllocationDbContext>
                (options => options.UseSqlServer(connectionStrings.ResourceAllocationApiContext));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseHsts();
            //}

            app.UseDeveloperExceptionPage();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ResourceAllocationDbContext>();
                context.Database.EnsureCreated();
                //context.Database.Migrate();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseCors("LocalCorsConfig");

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
