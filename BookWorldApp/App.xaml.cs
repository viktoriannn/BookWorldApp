using BookWorld.Data;
using BookWorld.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Markup;

namespace BookWorldApp;

public partial class App : Application
{
    private const string AppCulture = "ru-RU";

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        FrameworkElement.LanguageProperty.OverrideMetadata(
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(AppCulture)));

        try
        {
            var services = AppHost.Build();
            await services.GetRequiredService<DbInitializer>().InitializeAsync();
            Shutdown();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Не удалось запустить приложение: {ex.Message}", "BookWorld",
                MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown(1);
        }
    }
}
