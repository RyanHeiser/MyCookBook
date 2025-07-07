using MyCookBook.WPF.Stores.RecipeStores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Stores
{
    public class ModalNavigationStore : NavigationStore
    {
        public ModalNavigationStore(RecipeStore recipeStore, RecipeCategoryStore categoryStore, RecipeBookStore bookStore) : base(recipeStore, categoryStore, bookStore)
        {
        }

        public bool IsOpen => CurrentViewModel != null;

        public void Close()
        {
            Navigate(() => null);
        }
    }
}
