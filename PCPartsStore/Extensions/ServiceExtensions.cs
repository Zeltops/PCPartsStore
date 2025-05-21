using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PCPartsStore.Data;
using PCPartsStore.Policies.FirstTimeSetup;
using PCPartsStore.Repository;
using PCPartsStore.Repository.Interfaces;
using PCPartsStore.Services;
using PCPartsStore.Services.Interfaces;

namespace PCPartsStore.Extensions;

public static class ServiceExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        services.AddScoped<IProductImageRepository, ProductImageRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IOrderHistoryService, OrderHistoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ISearchService, SearchService>();
    }


    public static void AddUserAuthentication(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizationHandler, FirstTimeSetupHandler>();

        services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy("FirstTimeSetupComplete",
                policy => policy.Requirements.Add(new FirstTimeSetupRequirements(1)));
        });


        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromDays(1);
            options.Cookie.MaxAge = TimeSpan.FromDays(14);
            options.Cookie.IsEssential = true;
            options.Cookie.Name = "SessionData";
        });
    }
}