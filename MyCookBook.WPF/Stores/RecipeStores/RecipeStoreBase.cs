using MyCookBook.Domain.Models;
using MyCookBook.EntityFramework.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Stores.RecipeStores
{
    public abstract class RecipeStoreBase<T> where T : DomainObject
    {
        protected readonly IDataService<T> _dataService;
        protected List<T> _items;
        public IEnumerable<T> Items => _items;
        protected Lazy<Task> _initializeLazy;

        private T? _current;
        public T? Current { 
            get
            {
                return _current;
            }
            set
            {
                _current = value;
                OnCurrentChanged(_current);
            }
        }

        public event Action? FinishedLoading;
        public event Action<T>? NewCreated;
        public event Action<T>? ItemUpdated;
        public event Action? ItemDeleted;
        public event Action<T?>? CurrentChanged;

        protected RecipeStoreBase(IDataService<T> dataService)
        {
            _dataService = dataService;
            _items = new List<T>();
            _initializeLazy = new Lazy<Task>(Initialize);
        }

        public abstract Task<bool> Create(T item);
        public abstract Task<bool> Update(Guid Id, T item);
        public abstract Task<bool> Delete(Guid Id);

        public async Task Load()
        {
            try
            {
                await _initializeLazy.Value;
            }
            catch
            {
                _initializeLazy = new Lazy<Task>(Initialize);
                throw;
            }
            OnFinishedLoading();
        }

        protected virtual async Task Initialize()
        {
            IEnumerable<T> items = await _dataService.GetAll();

            _items.Clear();
            _items.AddRange(items);
        }

        protected void OnFinishedLoading()
        {
            FinishedLoading?.Invoke();
        }

        protected void OnNewCreated(T item)
        {
            NewCreated?.Invoke(item);
        }

        protected void OnItemUpdated(T item)
        {
            ItemUpdated?.Invoke(item);
        }

        protected void OnItemDeleted()
        {
            ItemDeleted?.Invoke();
        }

        protected void OnCurrentChanged(T? item)
        {
            CurrentChanged?.Invoke(item);
        }
    }
}
