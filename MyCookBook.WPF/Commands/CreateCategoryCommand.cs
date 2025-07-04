﻿using MyCookBook.Domain.Models;
using MyCookBook.WPF.Stores;
using MyCookBook.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Commands
{
    public class CreateCategoryCommand : AsyncCommandBase
    {
        private readonly CreateCategoryViewModel _viewModel;
        private readonly RecipeBookStore _recipeBookStore;

        public CreateCategoryCommand(CreateCategoryViewModel viewModel, RecipeBookStore recipeBookStore)
        {
            _viewModel = viewModel;
            _recipeBookStore = recipeBookStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            RecipeCategory category = new RecipeCategory(_viewModel.Name, new List<Recipe>());

            await _recipeBookStore.CreateCategory(category);
        }
    }
}
