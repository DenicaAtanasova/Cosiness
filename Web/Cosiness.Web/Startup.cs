namespace Cosiness.Web
{
    using Cosiness.Data;
    using Cosiness.Models;
    using Cosiness.Services.Data;
    using Cosiness.Services.Storage;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpsPolicy;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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
            services.AddDbContext<CosinessDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentity<CosinessUser, CosinessRole>(opt => 
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequiredLength = 6;
                opt.Password.RequiredUniqueChars = 0;
            })
                .AddEntityFrameworkStores<CosinessDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();
            
            services.AddControllersWithViews();
            services.AddTransient<IBaseNameOnlyEntityService<Category>, BaseNameOnlyEntityService<Category>>();
            services.AddTransient<IBaseNameOnlyEntityService<Color>, BaseNameOnlyEntityService<Color>>();
            services.AddTransient<IBaseNameOnlyEntityService<Material>, BaseNameOnlyEntityService<Material>>();
            services.AddTransient<IBaseNameOnlyEntityService<Set>, BaseNameOnlyEntityService<Set>>();
            services.AddTransient<IBaseNameOnlyEntityService<Dimension>, BaseNameOnlyEntityService<Dimension>>();
            services.AddTransient<IBaseNameOnlyEntityService<Town>, BaseNameOnlyEntityService<Town>>();
            services.AddTransient<IAddressService, AddressService>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<IFileStorage, BlobStorageService>();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IShoppingCartService, ShoppingCartService>();
            services.AddTransient<IOrderService, OrderService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}