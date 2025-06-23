using MyCookBook.Domain.Services.DTOConverters;
using MyCookBook.EntityFramework.DTOs;
using MyCookBook.EntityFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.Domain.Models
{
    public class RecipeBook
    {
        private readonly IDataService<RecipeCategoryDTO> _categoryDataService;
        private readonly RecipeCategoryDTOConverter _categoryDTOConverter;
        public List<RecipeCategory> _categories;
        public IEnumerable<RecipeCategory> Categories => _categories;

        public RecipeBook(IDataService<RecipeCategoryDTO> categoryDataService, RecipeCategoryDTOConverter categortyDTOConverter)
        {
            _categories = new List<RecipeCategory>();
            _categoryDataService = categoryDataService;
            _categoryDTOConverter = categortyDTOConverter;
        }

        /// <summary>
        /// Gets a category by Id
        /// </summary>
        /// <param name="Id">The Id of the category</param>
        /// <returns></returns>
        public async Task<RecipeCategory> GetCategory(Guid Id)
        {
            return _categoryDTOConverter.ConvertFromDTO(await _categoryDataService.Get(Id));
        }

        /// <summary>
        /// Gets all the categories in the ReservationBook.
        /// </summary>
        /// <returns>An IEnumerable comtaining the categories</returns>
        public async Task<IEnumerable<RecipeCategory>> GetAllCategories()
        {
            IEnumerable<RecipeCategoryDTO> dtos = await _categoryDataService.GetAll();
            
            return new List<RecipeCategory>(dtos.Select(d => _categoryDTOConverter.ConvertFromDTO(d)));
        }

        /// <summary>
        /// Adds a category in alphabetical order.
        /// </summary>
        /// <param name="recipe">The recipe to add.</param>
        /// <returns>The index at which the category was inserted</returns>
        public async Task AddRecipeCategory(RecipeCategory category)
        {
            RecipeCategoryDTO dto = _categoryDTOConverter.ConvertToDTO(category);
            await _categoryDataService.Create(dto);
        }

        /// <summary>
        /// Updates a category.
        /// </summary>
        /// <param name="Id">The Id of the category to update.</param>
        /// <param name="category">The new category replacing the old value</param>
        /// <returns>True if successful.</returns>
        public async Task<bool> UpdateRecipeCategory(Guid Id, RecipeCategory category)
        {
            RecipeCategoryDTO dto = _categoryDTOConverter.ConvertToDTO(category);
            return _categoryDTOConverter.UpdateFromDTO(await _categoryDataService.Update(Id, dto), category);
        }

        /// <summary>
        /// Removes a category by id.
        /// </summary>
        /// <param name="Id">The id of the recipe to remove.</param>
        public async Task<bool> RemoveRecipeCategory(Guid Id)
        {
            return await _categoryDataService.Delete(Id);
        }

        /// <summary>
        /// Clears the categories.
        /// TODO implement with database
        /// </summary>
        public void ClearRecipeCategories()
        {
            _categories.Clear();
        }
    }
}
