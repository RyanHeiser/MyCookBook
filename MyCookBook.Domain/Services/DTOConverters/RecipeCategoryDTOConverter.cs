using MyCookBook.Domain.Models;
using MyCookBook.EntityFramework.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.Domain.Services.DTOConverters
{
    public class RecipeCategoryDTOConverter
    {
        private readonly RecipeDTOConverter _recipeDTOConverter;

        public RecipeCategoryDTOConverter(RecipeDTOConverter recipeDTOConverter)
        {
            _recipeDTOConverter = recipeDTOConverter;
        }

        public RecipeCategoryDTO ConvertToDTO(RecipeCategory category)
        {
            return new RecipeCategoryDTO()
            {
                Id = category.Id,
                Name = category.Name,
                Recipes = category.Recipes.Select(r => _recipeDTOConverter.ConvertToDTO(r)).ToList() ?? new List<RecipeDTO>()
            };
        }

        public RecipeCategory ConvertFromDTO(RecipeCategoryDTO dto)
        {
            return new RecipeCategory(dto.Id, dto.Name, dto.Recipes.Select(r => _recipeDTOConverter.ConvertFromDTO(r)).ToList());
        }

        public bool UpdateFromDTO(RecipeCategoryDTO dto, RecipeCategory category)
        {
            if (dto.Id != category.Id)
            {
                return false;
            }

            category.Name = dto.Name;

            category.ClearRecipes();
            category.AddRecipes(dto.Recipes.Select(r => _recipeDTOConverter.ConvertFromDTO(r)));

            return true;
        }
    }
}
