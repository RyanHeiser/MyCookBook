﻿using MyCookBook.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Commands
{
    class AddToCollectionCommand<T> : CommandBase
    {
        private readonly ICollection<T> _collection;
        private readonly Func<T> _defaultFactory;

        public AddToCollectionCommand(ICollection<T> collection, Func<T> defaultFactory) 
        {
            _collection = collection;
            _defaultFactory = defaultFactory;
        }

        /// <summary>
        /// Add the command parameter to the collection on command execution. If the parameter is null or an invalid type, add the default object instead.
        /// </summary>
        /// <param name="parameter">The object to add to the collection</param>
        public override void Execute(object? parameter)
        {
            if (parameter == null || parameter is not T)
            {
                _collection.Add(_defaultFactory());
            } 
            else
            {
                _collection.Add((T) parameter);
            }
        }
    }
}
