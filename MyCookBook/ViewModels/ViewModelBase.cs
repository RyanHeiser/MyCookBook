﻿using MyCookBook.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        protected Recipe? _recipe;
        public Recipe? Recipe => _recipe;
        protected RecipeCategory? _category;
        public RecipeCategory? Category => _category;

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
