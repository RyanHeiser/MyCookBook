using MyCookBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        public Recipe? Recipe { get; protected set; }
        public RecipeCategory? Category { get; protected set; }
        public RecipeBook? Book { get; protected set; }

        protected bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }


        public bool HasErrorMessage => !String.IsNullOrEmpty(_errorMessage);

        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(HasErrorMessage));
            }
        }

       

        public event PropertyChangedEventHandler? PropertyChanged;

        public ViewModelBase()
        {
            IsLoading = false;
        }

        /// <summary>
        /// Invokes the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property which has changed.</param>
        protected void OnPropertyChanged(string? propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Dispose() { }

        public virtual void Update() 
        {
            ErrorMessage = String.Empty;
        }
    }
}
