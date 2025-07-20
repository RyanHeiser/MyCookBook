using MyCookBook.Domain.Models;
using MyCookBook.EntityFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Stores.RecipeStores
{
    public abstract class ChildRecipeStoreBase<TChild, TParent> : RecipeStoreBase<TChild> where TChild : ChildDomainObject where TParent : DomainObject
    {
        protected readonly ChildDataService<TChild> _childDataService;
        protected readonly RecipeStoreBase<TParent> _parentStore;

        protected ChildRecipeStoreBase(ChildDataService<TChild> dataService, RecipeStoreBase<TParent> parentStore) : base(dataService)
        {
            _childDataService = dataService;
            _parentStore = parentStore;

            parentStore.CurrentChanged += ParentCurrentChanged;
        }

        public abstract Task<bool> Move(TChild item, Guid newParentId);

        public virtual async Task<bool> Copy(TChild item, Guid newParentId)
        {
            TChild? copiedItem = await Duplicate(item.Id);

            if (copiedItem == null)
                return false;

            return await Move(copiedItem, newParentId);
        }

        private void ParentCurrentChanged(TParent? parent)
        {
            _initializeLazy = new Lazy<Task>(Initialize);
        }

        protected void UpdateParent()
        {
            _parentStore.Update(_parentStore.Current.Id, _parentStore.Current);
        }

        protected override async Task Initialize()
        {
            _items.Clear();

            if (_parentStore.Current == null)
                return;

            IEnumerable<TChild> items = await _childDataService.GetAllFromParent(_parentStore.Current.Id);
            _items.AddRange(items);
        }
    }
}
