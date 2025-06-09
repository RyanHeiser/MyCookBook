using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyCookBook.Models;
using MyCookBook.Services;
using MyCookBook.Services.Navigation;
using MyCookBook.Stores;
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

                // View Models
                services.AddTransient<CreateRecipeViewModel>();
                services.AddSingleton<Func<CreateRecipeViewModel>>(services => () => services.GetRequiredService<CreateRecipeViewModel>()); // function to navigate to CreateRecipeViewModel
                services.AddTransient<RecipeDisplayViewModel>();
                services.AddSingleton<Func<RecipeDisplayViewModel>>(services => () => services.GetRequiredService<RecipeDisplayViewModel>()); // function to navigate to RecipeDisplayViewModel
                services.AddSingleton<MainViewModel>();

                // Navigation Services
                services.AddSingleton<NavigationService<RecipeDisplayViewModel>>();
                services.AddSingleton<NavigationService<CreateRecipeViewModel>>();
                
                services.AddSingleton<NavigationStore>();
                services.AddSingleton<RecipeBookStore>();

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
        NavigationService<CreateRecipeViewModel> navigationService = _host.Services.GetRequiredService<NavigationService<CreateRecipeViewModel>>();
        navigationService.Navigate();

        MainWindow wnd = _host.Services.GetRequiredService<MainWindow>();
        wnd.Show();
        base.OnStartup(e);
    }
}

