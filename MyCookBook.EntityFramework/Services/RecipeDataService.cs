using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using MyCookBook.EntityFramework.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace MyCookBook.EntityFramework.Services
{
    public class RecipeDataService : IDataService<RecipeDTO>
    {
        private readonly CategoryDataService _categoriesDataService;
        private readonly Guid _categoryId;

        public RecipeDataService(Guid CategoryId, CategoryDataService categoriesDataService)
        {
            _categoriesDataService = categoriesDataService;
            _categoryId = CategoryId;
        }

        public async Task<RecipeDTO> Create(RecipeDTO entity)
        {
            RecipeCategoryDTO category = await _categoriesDataService.Get(_categoryId);

            category.Recipes ??= new List<RecipeDTO>();

            category.Recipes.Add(entity);
            await _categoriesDataService.Update(_categoryId, category);

            return entity;
        }

        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<RecipeDTO> Get(Guid id)
        {
            RecipeCategoryDTO category = await _categoriesDataService.Get(_categoryId);
            foreach (RecipeDTO recipe in category.Recipes)
            {
                if (recipe.Id == id)
                {
                    return recipe;
                }
            }
            return category.Recipes.ElementAt(0);
        }

        public async Task<IEnumerable<RecipeDTO>> GetAll()
        {   
            RecipeCategoryDTO category = await _categoriesDataService.Get(_categoryId);
            return category.Recipes;
        }

        public Task<RecipeDTO> Update(Guid id, RecipeDTO entity)
        {
            throw new NotImplementedException();
        }
    }
}
