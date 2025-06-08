using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyCookBook.Models;
using MyCookBook.Services;
using MyCookBook.ViewModels;
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
                services.AddSingleton((s) => new RecipeBook(new List<RecipeCategory>()));

                services.AddTransient<CreateRecipeViewModel>();
                services.AddTransient<RecipeDisplayViewModel>();
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<Func<Type, ViewModelBase>>(services => viewModelType => (ViewModelBase) services.GetRequiredService(viewModelType));

                services.AddSingleton<NavigationService>();

                services.AddSingleton(s => new MainWindow()
                {
                    DataContext = s.GetRequiredService<MainViewModel>()
                });
            })
            .Build();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        _host.Start();
        NavigationService navigationService = _host.Services.GetRequiredService<NavigationService>();
        navigationService.NavigateTo<CreateRecipeViewModel>();
        MainWindow wnd = _host.Services.GetRequiredService<MainWindow>();
        wnd.Show();
        base.OnStartup(e);
    }
}

