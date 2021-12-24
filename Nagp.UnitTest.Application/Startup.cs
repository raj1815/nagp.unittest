using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Nagp.UnitTest.Business;
using Nagp.UnitTest.Business.Common;
using Nagp.UnitTest.Business.Interface;
using Nagp.UnitTest.EntityFrameworkCore;
using Nagp.UnitTest.EntityFrameworkCore.Model;
using Nagp.UnitTest.EntityFrameworkCore.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Nagp.UnitTest.Application
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

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IStockManager, StockManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IRepository<User>, GenericRepository<User>>();
            services.AddScoped<IRepository<Stock>, GenericRepository<Stock>>();
            services.AddScoped<IRepository<HoldingShare>, GenericRepository<HoldingShare>>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWrapper, Wrapper>();
      
            //services.AddDbContext<eTraderDBContext>(opt => opt.UseInMemoryDatabase(databaseName: "eTraderDB"));
            //services.AddScoped<eTraderDBContext>();
            // Add framework services.
            services.AddDbContext<eTraderDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Trade", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product.API v1"));

            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
