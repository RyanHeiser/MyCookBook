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

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Invokes the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property which has changed.</param>
        protected void OnPropertyChanged(string? propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Dispose() { }
    }
}
