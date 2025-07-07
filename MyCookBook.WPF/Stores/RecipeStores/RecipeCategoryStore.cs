using MyCookBook.Domain.Models;
using MyCookBook.EntityFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Stores.RecipeStores
{
    public class RecipeCategoryStore : ChildRecipeStoreBase<RecipeCategory, RecipeBook>
    {
        public RecipeCategoryStore(ChildDataService<RecipeCategory> dataService, RecipeBookStore categoryStore) : base(dataService, categoryStore)
        {
        }

        public override async Task<bool> Create(RecipeCategory category)
        {
            if (_parentStore.Current == null)
            {
                return false;
            }
            await _dataService.Create(category);
            _parentStore.Current.AddCategory(category);
            _items.Add(category);

            UpdateParent();
            OnNewCreated(category);
            return true;
        }

        public override async Task<bool> Delete(Guid Id)
        {
            if (_parentStore.Current != null && await _dataService.Delete(Id))
            {
                _parentStore.Current.RemoveCategory(Id);

                RecipeCategory item = _items.FirstOrDefault(c => c.Id == Id);
                if (item != null)
                    _items.Remove(item);

                UpdateParent();
                return true;
            }
            return false;
        }

        public override async Task<bool> Update(Guid Id, RecipeCategory category)
        {
            try
            {
                if (_parentStore.Current == null)
                    return false;

                RecipeCategory updatedCategory = await _dataService.Update(Id, category);
                _parentStore.Current.RemoveCategory(Id);
                _parentStore.Current.AddCategory(updatedCategory);

                _items.Add(category);
                RecipeCategory item = _items.FirstOrDefault(c => c.Id == Id);
                if (item != null)
                    _items.Remove(item);

                UpdateParent();
                OnItemUpdated(updatedCategory);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
