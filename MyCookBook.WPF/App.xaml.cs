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
using MyCookBook.EntityFramework.Services;
using Microsoft.EntityFrameworkCore;
using MyCookBook.WPF.Stores.RecipeStores;
using MyCookBook.WPF.ViewModels.Modals;

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
                services.AddSingleton<RecipeBook>();

                // MainViewModel
                services.AddSingleton<MainViewModel>();

                // Navigation service
                services.AddSingleton<INavigationService>(services => RecipeBookListingNavigationService(services));

                // Services
                services.AddSingleton<IDataService<RecipeBook>, DataService<RecipeBook>>();
                services.AddSingleton<ChildDataService<RecipeCategory>>();
                services.AddSingleton<ChildDataService<Recipe>>();
                services.AddSingleton<ChildDataService<RecipeImage>>();

                // DB Context Factory
                services.AddSingleton<MyCookBookDbContextFactory>();

                // Stores
                services.AddSingleton<NavigationStore>();
                services.AddSingleton<ModalNavigationStore>();
                services.AddSingleton<RecipeBookStore>();
                services.AddSingleton<RecipeCategoryStore>();
                services.AddSingleton<RecipeStore>(); 
                services.AddSingleton<RecipeImageStore>();

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

        MyCookBookDbContextFactory contextFactory = _host.Services.GetRequiredService<MyCookBookDbContextFactory>();
        using (MyCookBookDbContext context = contextFactory.CreateDbContext())
        {
            if (context.Database.EnsureCreated())
                context.Database.Migrate();
        }

        INavigationService navigationService = _host.Services.GetRequiredService<INavigationService>();
        navigationService.Navigate();

        MainWindow wnd = _host.Services.GetRequiredService<MainWindow>();
        wnd.Show();
        base.OnStartup(e);
    }

    #region Navigation Services
    private INavigationService RecipeBookListingNavigationService(IServiceProvider services)
    {
        NavigationStore navigationStore = services.GetRequiredService<NavigationStore>();

        return new NavigationService<RecipeBookListingViewModel>(navigationStore, () => RecipeBookListingViewModel(services));
    }

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

    private INavigationService PreviousNavigationService(IServiceProvider services)
    {
        return new PreviousNavigationService(services.GetRequiredService<NavigationStore>());
    }
    #endregion


    #region Modal Navigation Services
    private INavigationService CreateRecipeBookNavigationService(IServiceProvider services)
    {
        return new ModalNavigationService<CreateRecipeBookViewModel>(services.GetRequiredService<ModalNavigationStore>(), () => CreateRecipeBookViewModel(services));
    }

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
    private RecipeBookListingViewModel RecipeBookListingViewModel(IServiceProvider services)
    {
        return new RecipeBookListingViewModel(services.GetRequiredService<RecipeBookStore>(), CreateRecipeBookNavigationService(services),
            CategoryListingNavigationService(services));
    }

    private CategoryListingViewModel CategoryListingViewModel(IServiceProvider services)
    {
        return new CategoryListingViewModel(services.GetRequiredService<RecipeBookStore>(), services.GetRequiredService<RecipeCategoryStore>(),
            CreateCategoryNavigationService(services), CreateRecipeBookNavigationService(services), RecipeListingNavigationService(services),
            PreviousNavigationService(services));
    }

    private RecipeListingViewModel RecipeListingViewModel(IServiceProvider services)
    {
        return new RecipeListingViewModel(services.GetRequiredService<RecipeCategoryStore>(), services.GetRequiredService<RecipeStore>(),
            CreateRecipeNavigationService(services), CreateCategoryNavigationService(services), RecipeDisplayNavigationService(services), PreviousNavigationService(services));
    }

    private CreateRecipeViewModel CreateRecipeViewModel(IServiceProvider services)
    {
        return new CreateRecipeViewModel(services.GetRequiredService<RecipeCategoryStore>(), services.GetRequiredService<RecipeStore>(),
            services.GetRequiredService<RecipeImageStore>(), RecipeDisplayNavigationService(services), PreviousNavigationService(services));
    }

    private RecipeDisplayViewModel RecipeDisplayViewModel(IServiceProvider services)
    {
        return new RecipeDisplayViewModel(services.GetRequiredService<RecipeCategoryStore>(), services.GetRequiredService<RecipeStore>(),
            services.GetRequiredService<RecipeImageStore>(), CreateRecipeNavigationService(services), PreviousNavigationService(services));
    }

    #endregion

    #region Modal View Models
    private CreateRecipeBookViewModel CreateRecipeBookViewModel(IServiceProvider services)
    {
        return new CreateRecipeBookViewModel(services.GetRequiredService<RecipeBookStore>(), services.GetRequiredService<RecipeCategoryStore>(), CloseModalNavigationService(services));
    }

    private CreateCategoryViewModel CreateCategoryViewModel(IServiceProvider services)
    {
        return new CreateCategoryViewModel(services.GetRequiredService<RecipeBookStore>(), services.GetRequiredService<RecipeCategoryStore>(), CloseModalNavigationService(services));
    } 
    #endregion
}

