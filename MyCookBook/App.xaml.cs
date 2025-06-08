using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyCookBook.Models;
using MyCookBook.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace MyCookBook;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<MainWindow>();

                services.AddSingleton<RecipeBook>();
                services.AddTransient<RecipeCategory>();
                services.AddTransient<Recipe>();
            })
            .Build();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        _host.Start();

        MainWindow wnd = _host.Services.GetRequiredService<MainWindow>();
        wnd.Show();
        base.OnStartup(e);
    }
}

