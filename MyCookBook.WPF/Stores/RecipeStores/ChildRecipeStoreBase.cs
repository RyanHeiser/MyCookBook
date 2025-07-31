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

        /// <summary>
        /// Copies an item to a new parent.
        /// </summary>
        /// <param name="item">The item to copy.</param>
        /// <param name="newParentId">The Id of the new parent.</param>
        /// <returns>True if copied successfully</returns>
        public virtual async Task<bool> Copy(TChild item, Guid newParentId)
        {
            TChild? copiedItem = await Duplicate(item.Id);

            if (copiedItem == null)
                return false;

            return await Move(copiedItem, newParentId);
        }

        /// <summary>
        /// Loads new items when parent is changed.
        /// </summary>
        /// <param name="parent"></param>
        private void ParentCurrentChanged(TParent? parent)
        {
            _initializeLazy = new Lazy<Task>(Initialize);
        }

        /// <summary>
        /// Updates the parent in the database.
        /// </summary>
        protected void UpdateParent()
        {
            _parentStore.Update(_parentStore.Current.Id, _parentStore.Current);
        }

        /// <summary>
        /// Loads items from database.
        /// </summary>
        /// <returns></returns>
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
