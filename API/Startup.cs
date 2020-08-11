using AutoMapper;
using LaYumba.Functional;
using Logic;
using Logic.AppServices;
using Logic.Data;
using Logic.Models;
using Logic.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Logic.AppServices.AddAddressCommand;
using static Logic.AppServices.CreateCustomerCommand;
using static Logic.AppServices.DeleteCustomerCommand;
using static Logic.AppServices.EditCustomerInfoCommand;
using static Logic.AppServices.GetAllCustomerQuery;
using static Logic.AppServices.GetCustomerQuery;
using static Logic.AppServices.MarkAddressPrimaryCommand;
using static Logic.AppServices.RemoveAddressCommand;
using Unit = System.ValueTuple;

namespace API
{
    public class Startup
    {
        private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = new ConnectionString(
                Configuration.GetConnectionString("ApplicationDbContext"));
            services.AddSingleton<ConnectionString>(connectionString);
            services.AddSingleton<DbContextFactory>();
            services.AddSingleton<Messages>();

            //Handlers
            services.AddTransient<ICommandHandler<CreateCustomerCommand, Task<Validation<Customer>>>,
                CreateCustomerCommandHandler>();
            services.AddTransient<ICommandHandler<EditCustomerInfoCommand, Task<Validation<Unit>>>,
                EditCustomerInfoCommandHandler>();
            services.AddTransient<ICommandHandler<DeleteCustomerCommand, Task<Validation<Unit>>>,
                DeleteCustomerCommandHandler>();
            services.AddTransient<ICommandHandler<AddAddressCommand, Task<Validation<Address>>>,
                AddAddressCommandHandler>();
            services.AddTransient<ICommandHandler<MarkAddressPrimaryCommand, Task<Validation<Unit>>>,
                MarkPrimaryAddressCommandHandler>();
            services.AddTransient<ICommandHandler<RemoveAddressCommand, Task<Validation<Unit>>>,
                RemoveAddressCommandHandler>();

            //Queries
            services.AddTransient<IQueryHandler<GetCustomerQuery, Task<Validation<Customer>>>,
                GetCustomerQueryHandler>();
            services.AddTransient<IQueryHandler<GetAllCustomerQuery, Task<Validation<IReadOnlyCollection<Customer>>>>,
                GetAllCustomerQueryHandler>();

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("https://localhost:44311", "https://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(MyAllowSpecificOrigins);

            app.UseSwagger();

            app.UseHttpsRedirection();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}