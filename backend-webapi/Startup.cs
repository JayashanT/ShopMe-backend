using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using backend_webapi.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using webapi.Dtos;
using webapi.Entities;
using webapi.Helpers;
using webapi.Repositories;
using webapi.Services;
using webapi.ViewModels;

namespace backend_webapi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //added JWTSerttings
            services.Configure<AppSettings>(Configuration.GetSection("JWTSettings"));

            services.AddOptions();

            services.AddDbContext<ShopmeDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            services.AddScoped<ICommonRepository<Customer>, CommonRepository<Customer>>();
            services.AddScoped<ICommonRepository<Deliverer>, CommonRepository<Deliverer>>();
            services.AddScoped<ICommonRepository<Admin>, CommonRepository<Admin>>();
            services.AddScoped<ICommonRepository<Seller>, CommonRepository<Seller>>();
            services.AddScoped<ICommonRepository<Product>, CommonRepository<Product>>();
            services.AddScoped<ICommonRepository<Order>, CommonRepository<Order>>();
            services.AddScoped<ICommonRepository<OrderItem>, CommonRepository<OrderItem>>();
            services.AddScoped<ICommonRepository<OrderItemProduct>, CommonRepository<OrderItemProduct>>();
            services.AddScoped<ICommonRepository<Payment>, CommonRepository<Payment>>();
            services.AddScoped<ICommonRepository<Location>, CommonRepository<Location>>();
            services.AddScoped<ICommonRepository<Login>, CommonRepository<Login>>();
            

            services.AddScoped<ISellerService, SellerService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IDelivererService, DelivererService>();
            services.AddScoped<ILocationService, LocationService>();
            
            services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy",
                builder =>
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            //services.AddAutoMapper();


            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("JWTSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            
            //configure DI for application services
            services.AddScoped<IUserService, UserService>();
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();


            AutoMapper.Mapper.Initialize(mapper =>
            {
                mapper.CreateMap<Customer, CustomerDto>().ReverseMap();
                mapper.CreateMap<Seller, SellerDto>().ReverseMap();
                mapper.CreateMap<Deliverer, DelivererDto>().ReverseMap();
                mapper.CreateMap<Admin, AdminDto>().ReverseMap();
                mapper.CreateMap<Product, ProductDto>().ReverseMap();
                mapper.CreateMap<Order, OrderDto>().ReverseMap();
                mapper.CreateMap<OrderItem, OrderItemDto>().ReverseMap();
                mapper.CreateMap<OrderItemProduct, OrderItemProductDto>().ReverseMap();
                mapper.CreateMap<Payment, PaymentDto>().ReverseMap();
                mapper.CreateMap<Category, CategoryDto>().ReverseMap();
                mapper.CreateMap<Location, LocationDto>().ReverseMap();

                mapper.CreateMap<Login, LoginVM>().ReverseMap();

            });


            // global cors policy
            app.UseCors("MyPolicy");

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseHttpsRedirection();
            app.UseWebSockets();
            //app.UseMvc();
        }
    }
}

