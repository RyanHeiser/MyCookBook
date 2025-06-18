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
                // Model
                services.AddSingleton((s) => new RecipeBook(new List<RecipeCategory>()));

                // View Models
                services.AddTransient<CategoryListingViewModel>();
                services.AddSingleton<Func<CategoryListingViewModel>>(services => () => services.GetRequiredService<CategoryListingViewModel>()); // function to navigate to RecipeListingViewModel
                services.AddTransient<RecipeListingViewModel>();
                services.AddSingleton<Func<RecipeListingViewModel>>(services => () => services.GetRequiredService<RecipeListingViewModel>()); // function to navigate to RecipeListingViewModel
                services.AddTransient<CreateRecipeViewModel>();
                services.AddSingleton<Func<CreateRecipeViewModel>>(services => () => services.GetRequiredService<CreateRecipeViewModel>()); // function to navigate to CreateRecipeViewModel
                services.AddTransient<RecipeDisplayViewModel>();
                services.AddSingleton<Func<RecipeDisplayViewModel>>(services => () => services.GetRequiredService<RecipeDisplayViewModel>()); // function to navigate to RecipeDisplayViewModel
                services.AddSingleton<MainViewModel>();

                // Navigation Services
                services.AddSingleton<NavigationService<CategoryListingViewModel>>();
                services.AddSingleton<NavigationService<RecipeListingViewModel>>();
                services.AddSingleton<NavigationService<RecipeDisplayViewModel>>();
                services.AddSingleton<NavigationService<CreateRecipeViewModel>>();
                
                // Stores
                services.AddSingleton<NavigationStore>();
                services.AddSingleton<RecipeBookStore>();
                services.AddSingleton<RecipeStore>();

                // MainWindow
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

        #region Temp Scaffolding
        RecipeStore recipeStore = _host.Services.GetRequiredService<RecipeStore>();
        RecipeBookStore recipeBookStore = _host.Services.GetRequiredService<RecipeBookStore>();
        RecipeCategory category = new RecipeCategory("My Category", new List<Recipe>());
        recipeStore.CurrentCategory = category;
        recipeBookStore.CreateRecipeCategory(category); 
        #endregion

        NavigationService<RecipeListingViewModel> navigationService = _host.Services.GetRequiredService<NavigationService<RecipeListingViewModel>>();
        navigationService.Navigate();

        MainWindow wnd = _host.Services.GetRequiredService<MainWindow>();
        wnd.Show();
        base.OnStartup(e);
    }
}

