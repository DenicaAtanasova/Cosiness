namespace Cosiness.Web
{
    using Cosiness.Data;
    using Cosiness.Models;
    using Cosiness.Services.Data;
    using Cosiness.Services.Mapping;
    using Cosiness.Services.Storage;
    using Cosiness.Web.InputModels.Products;
    using Cosiness.Web.ViewModels;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System.Reflection;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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
            services.AddTransient<IStorageService, StorageService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(
                typeof(ErrorViewModel).GetTypeInfo().Assembly,
                typeof(ProductInputModel).GetTypeInfo().Assembly);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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
                    name: "area",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}