using BookWorldApp.Data;
using BookWorldApp.Services;
using BookWorldApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookWorldApp.Infrastructure;

public static class AppHost
{
    public static IServiceProvider Build()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile(BookStoreContextFactory.SettingsFile, optional: false)
            .Build();

        var connectionString = configuration.GetConnectionString(BookStoreContextFactory.ConnectionName)
            ?? throw new InvalidOperationException(
                $"В {BookStoreContextFactory.SettingsFile} не задана строка подключения «{BookStoreContextFactory.ConnectionName}».");

        var services = new ServiceCollection();
        services.AddDbContextFactory<BookStoreContext>(options => options.UseSqlServer(connectionString));
        services.AddSingleton<DbInitializer>();
        services.AddSingleton<ICatalogService, CatalogService>();
        services.AddSingleton<UserSession>();
        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<ICatalogService, CatalogService>();
        services.AddSingleton<IReferenceService, ReferenceService>();
        services.AddSingleton<IAvailabilityService, AvailabilityService>();
        services.AddSingleton<ISaleService, SaleService>();
        services.AddSingleton<IChequeService, ChequeService>();
        services.AddSingleton<IReplenishmentService, ReplenishmentService>();

        services.AddSingleton<IWindowService, WindowService>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<CashierViewModel>();
        services.AddTransient<OrderCreateViewModel>();

        return services.BuildServiceProvider();
    }
}
