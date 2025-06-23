using MyCookBook.Domain.Models;
using MyCookBook.EntityFramework.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.Domain.Services.DTOConverters
{
    public class RecipeDTOConverter
    {
        public RecipeDTO ConvertToDTO(Recipe recipe)
        {
            return new RecipeDTO()
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Minutes = recipe.Minutes,
                Servings = recipe.Servings,
                Ingredients = (List<string>)(recipe.Ingredients ?? new List<string>()),
                Directions = (List<string>)(recipe.Directions ?? new List<string>()),
            };
        }

        public Recipe ConvertFromDTO(RecipeDTO dto)
        {
            return new Recipe(dto.Id, dto.Name, dto.Minutes, dto.Servings, (List<string>)dto.Ingredients, (List<string>)dto.Directions);
        }

        public bool UpdateFromDTO(RecipeDTO dto, Recipe recipe)
        {
            if (dto.Id != recipe.Id)
            {
                return false;
            }

            recipe.Name = dto.Name;
            recipe.Minutes = dto.Minutes;
            recipe.Servings = dto.Servings;

            recipe.ClearIngredients();
            recipe.AddIngredients(dto.Ingredients);

            recipe.ClearDirections();
            recipe.AddDirections(dto.Directions);

            return true;
        }
    }
}
