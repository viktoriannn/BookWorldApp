using BookWorld.Data;
using BookWorldApp.Data;
using BookWorldApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookWorld.Infrastructure;

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

        return services.BuildServiceProvider();
    }
}
