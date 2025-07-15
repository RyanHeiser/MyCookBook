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
using System.Windows.Input;
using MyCookBook.WPF.Commands;

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
                services.AddSingleton<RecipeBookIODataService>();
                services.AddSingleton<IDataService<RecipeBook>, DataService<RecipeBook>>();
                services.AddSingleton<ChildDataService<RecipeCategory>>();
                services.AddSingleton<ChildDataService<Recipe>>();
                services.AddSingleton<ChildDataService<RecipeImage>>();

                // DB Context Factory
                services.AddSingleton<MyCookBookDbContextFactory>();

                // Navigation Stores
                services.AddSingleton<NavigationStore>();
                services.AddSingleton<ModalNavigationStore>();

                // Recipe Stores
                services.AddSingleton<RecipeStoreBase<RecipeBook>, RecipeBookStore>();
                services.AddSingleton<RecipeStoreBase<RecipeCategory>, RecipeCategoryStore>();
                services.AddSingleton<RecipeStoreBase<Recipe>, RecipeStore>(); 
                services.AddSingleton<RecipeStoreBase<RecipeImage>, RecipeImageStore>();

                // Other Stores
                services.AddSingleton<DeleteStore>();

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
            if (context != null && context.Database != null)
            {
                context.Database.Migrate();
            }
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

    private INavigationService DeleteNavigationService<T>(IServiceProvider services, string itemToDelete) where T : DomainObject
    {
        return new ModalNavigationService<DeleteViewModel>(services.GetRequiredService<ModalNavigationStore>(), () => DeleteViewModel<T>(services, itemToDelete));
    }

    private INavigationService DeleteNavigationService<T>(IServiceProvider services, string itemToDelete, ICommand commands) where T : DomainObject
    {
        return new ModalNavigationService<DeleteViewModel>(services.GetRequiredService<ModalNavigationStore>(), () => DeleteViewModel<T>(services, itemToDelete, commands));
    }

    private INavigationService CloseModalNavigationService(IServiceProvider services)
    {
        return new CloseModalNavigationService(services.GetRequiredService<ModalNavigationStore>());
    }
    #endregion


    #region View Models
    private RecipeBookListingViewModel RecipeBookListingViewModel(IServiceProvider services)
    {
        return new RecipeBookListingViewModel(services.GetRequiredService<RecipeStoreBase<RecipeBook>>(), services.GetRequiredService<DeleteStore>(), 
            services.GetRequiredService<RecipeBookIODataService>(), CreateRecipeBookNavigationService(services), CategoryListingNavigationService(services), 
            DeleteNavigationService<RecipeBook>(services, "recipe book"));
    }

    private CategoryListingViewModel CategoryListingViewModel(IServiceProvider services)
    {
        return new CategoryListingViewModel(services.GetRequiredService<RecipeStoreBase<RecipeBook>>(), services.GetRequiredService<RecipeStoreBase<RecipeCategory>>(), 
            services.GetRequiredService<DeleteStore>(), CreateCategoryNavigationService(services), CreateRecipeBookNavigationService(services), 
            RecipeListingNavigationService(services), DeleteNavigationService<RecipeCategory>(services, "category"), 
            DeleteNavigationService<RecipeBook>(services, "recipe book", new NavigateCommand(PreviousNavigationService(services))), 
            PreviousNavigationService(services));
    }

    private RecipeListingViewModel RecipeListingViewModel(IServiceProvider services)
    {
        return new RecipeListingViewModel(services.GetRequiredService<RecipeStoreBase<RecipeCategory>>(), services.GetRequiredService<RecipeStoreBase<Recipe>>(),
            services.GetRequiredService<DeleteStore>(), CreateRecipeNavigationService(services), CreateCategoryNavigationService(services), RecipeDisplayNavigationService(services),
            DeleteNavigationService<Recipe>(services, "recipe"), 
            DeleteNavigationService<RecipeCategory>(services, "category", new NavigateCommand(PreviousNavigationService(services))),
            PreviousNavigationService(services));
    }

    private CreateRecipeViewModel CreateRecipeViewModel(IServiceProvider services)
    {
        return new CreateRecipeViewModel(services.GetRequiredService<RecipeStoreBase<RecipeCategory>>(), services.GetRequiredService<RecipeStoreBase<Recipe>>(),
            services.GetRequiredService<RecipeStoreBase<RecipeImage>>(), RecipeDisplayNavigationService(services), PreviousNavigationService(services));
    }

    private RecipeDisplayViewModel RecipeDisplayViewModel(IServiceProvider services)
    {
        return new RecipeDisplayViewModel(services.GetRequiredService<RecipeStoreBase<RecipeCategory>>(), services.GetRequiredService<RecipeStoreBase<Recipe>>(),
            services.GetRequiredService<RecipeStoreBase<RecipeImage>>(), services.GetRequiredService<DeleteStore>(), CreateRecipeNavigationService(services), 
            DeleteNavigationService<Recipe>(services, "recipe", new NavigateCommand(PreviousNavigationService(services))), PreviousNavigationService(services));
    }

    #endregion

    #region Modal View Models
    private CreateRecipeBookViewModel CreateRecipeBookViewModel(IServiceProvider services)
    {
        return new CreateRecipeBookViewModel(services.GetRequiredService<RecipeStoreBase<RecipeBook>>(), services.GetRequiredService<RecipeStoreBase<RecipeCategory>>(), CloseModalNavigationService(services));
    }

    private CreateCategoryViewModel CreateCategoryViewModel(IServiceProvider services)
    {
        return new CreateCategoryViewModel(services.GetRequiredService<RecipeStoreBase<RecipeBook>>(), services.GetRequiredService<RecipeStoreBase<RecipeCategory>>(), CloseModalNavigationService(services));
    }

    private DeleteViewModel DeleteViewModel<T> (IServiceProvider services, string itemToDelete) where T : DomainObject
    {
        return new DeleteViewModel(services.GetRequiredService<DeleteStore>(), itemToDelete, CloseModalNavigationService(services), new DeleteCommand<T>(services.GetRequiredService<RecipeStoreBase<T>>()));
    }

    private DeleteViewModel DeleteViewModel<T>(IServiceProvider services, string itemToDelete, ICommand commands) where T : DomainObject
    {
        return new DeleteViewModel(services.GetRequiredService<DeleteStore>(), itemToDelete, CloseModalNavigationService(services), new DeleteCommand<T>(services.GetRequiredService<RecipeStoreBase<T>>()), commands);
    }
    #endregion
}

