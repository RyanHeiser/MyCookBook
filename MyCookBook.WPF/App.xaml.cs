using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyCookBook.EntityFramework;
using MyCookBook.Domain.Models;
using MyCookBook.WPF.Services;
using MyCookBook.WPF.Services.Navigation;
using MyCookBook.WPF.Stores;
using MyCookBook.WPF.ViewModels;
using MyCookBook.WPF.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace MyCookBook.WPF;

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
                // RecipeBook Model
                services.AddSingleton((s) => new RecipeBook(new List<RecipeCategory>()));

                // MainViewModel
                services.AddSingleton<MainViewModel>();

                // Navigation service
                services.AddSingleton<INavigationService>(services => CategoryListingNavigationService(services));

                // Stores
                services.AddSingleton<NavigationStore>();
                services.AddSingleton<ModalNavigationStore>();
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

        INavigationService navigationService = _host.Services.GetRequiredService<INavigationService>();
        navigationService.Navigate();

        MainWindow wnd = _host.Services.GetRequiredService<MainWindow>();
        wnd.Show();
        base.OnStartup(e);
    }

    #region Navigation Services
    private INavigationService CategoryListingNavigationService(IServiceProvider services)
    {
        NavigationStore navigationStore = services.GetRequiredService<NavigationStore>();

        return new NavigationService<CategoryListingViewModel>(navigationStore, () => CategoryListingViewModel(services));
    }

    private INavigationService RecipeListingNavigationService(IServiceProvider services)
    {
        return new NavigationService<RecipeListingViewModel>(services.GetRequiredService<NavigationStore>(), () => RecipeListingViewModel(services));
    }

    private INavigationService CreateRecipeNavigationService(IServiceProvider services)
    {
        return new NavigationService<CreateRecipeViewModel>(services.GetRequiredService<NavigationStore>(), () => CreateRecipeViewModel(services));
    }

    private INavigationService RecipeDisplayNavigationService(IServiceProvider services)
    {
        return new NavigationService<RecipeDisplayViewModel>(services.GetRequiredService<NavigationStore>(), () => RecipeDisplayViewModel(services));
    }
    #endregion


    #region Modal Navigation Services
    private INavigationService CreateCategoryNavigationService(IServiceProvider services)
    {
        return new ModalNavigationService<CreateCategoryViewModel>(services.GetRequiredService<ModalNavigationStore>(), () => CreateCategoryViewModel(services));
    }

    private INavigationService CloseModalNavigationService(IServiceProvider services)
    {
        return new CloseModalNavigationService(services.GetRequiredService<ModalNavigationStore>());
    }
    #endregion


    #region View Models
    private CategoryListingViewModel CategoryListingViewModel(IServiceProvider services)
    {
        return new CategoryListingViewModel(services.GetRequiredService<RecipeBookStore>(), services.GetRequiredService<RecipeStore>(),
            CreateCategoryNavigationService(services), RecipeListingNavigationService(services));
    }

    private RecipeListingViewModel RecipeListingViewModel(IServiceProvider services)
    {
        return new RecipeListingViewModel(services.GetRequiredService<RecipeBookStore>(), services.GetRequiredService<RecipeStore>(),
            CreateRecipeNavigationService(services), RecipeDisplayNavigationService(services), CategoryListingNavigationService(services));
    }

    private CreateRecipeViewModel CreateRecipeViewModel(IServiceProvider services)
    {
        return new CreateRecipeViewModel(services.GetRequiredService<RecipeBookStore>(), services.GetRequiredService<RecipeStore>(),
            RecipeListingNavigationService(services), RecipeDisplayNavigationService(services));
    }

    private RecipeDisplayViewModel RecipeDisplayViewModel(IServiceProvider services)
    {
        return new RecipeDisplayViewModel(services.GetRequiredService<RecipeBookStore>(), services.GetRequiredService<RecipeStore>(),
            RecipeListingNavigationService(services));
    }

    private CreateCategoryViewModel CreateCategoryViewModel(IServiceProvider services)
    {
        return new CreateCategoryViewModel(services.GetRequiredService<RecipeBookStore>(), CloseModalNavigationService(services));
    } 
    #endregion
}

