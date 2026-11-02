using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BookWorldApp.Data;

public sealed class BookStoreContextFactory : IDesignTimeDbContextFactory<BookStoreContext>
{
    public const string ConnectionName = "BookStore";
    public const string SettingsFile = "appsettings.json";

    public BookStoreContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(SettingsFile, optional: false)
            .Build();

        var options = new DbContextOptionsBuilder<BookStoreContext>()
            .UseSqlServer(configuration.GetConnectionString(ConnectionName))
            .Options;
        return new BookStoreContext(options);
    }
}
