using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.Domain.Models
{
    public class Recipe : ChildDomainObject
    {        
        public string Name { get; set; }
        public int Minutes { get; set; }
        public int Servings { get; set; }

        public byte[]? RawThumbnailData { get; set; }
        public RecipeImage? Image { get; set; }

        private List<string> _ingredients;
        public IEnumerable<string> Ingredients { get { return _ingredients; } set { _ingredients = value.ToList(); } }

        private List<string> _directions;
        public IEnumerable<string> Directions { get { return _directions; } set { _ingredients = value.ToList(); } }

        public Recipe(string name, int minutes, int servings)
        {
            Id = Guid.NewGuid();
            Name = name;
            Minutes = minutes;
            Servings = servings;
            _ingredients = new List<string>();
            _directions = new List<string>();
        }

        public Recipe(string name, int minutes, int servings, byte[] rawThumbnailData, List<string> ingredients, List<string> directions)
        {
            Id = Guid.NewGuid();
            Name = name;
            Minutes = minutes;
            Servings = servings;
            RawThumbnailData = rawThumbnailData;
            _ingredients = ingredients;
            _directions = directions;
        }

        public Recipe(string name, int minutes, int servings, byte[] rawThumbnailData, List<string> ingredients, List<string> directions, Guid parentId)
        {
            Id = Guid.NewGuid();
            Name = name;
            Minutes = minutes;
            Servings = servings;
            RawThumbnailData = rawThumbnailData;
            _ingredients = ingredients;
            _directions = directions;
            ParentId = parentId;
        }

        /// <summary>
        /// Adds an ingredient to end of _ingredients list.
        /// </summary>
        /// <param name="ingredient">The ingredient to add.</param>
        public void AddIngredient(string ingredient)
        {
            _ingredients.Add(ingredient);
        }

        /// <summary>
        /// Adds a range of ingredients.
        /// </summary>
        /// <param name="ingredients">The ingredients to add.</param>
        public void AddIngredients(IEnumerable<string> ingredients)
        {
            _ingredients.AddRange(ingredients);
        }

        /// <summary>
        /// Inserts an ingredient at the specified index of the _ingredients list.
        /// </summary>
        /// <param name="ingredient">The ingredient to add.</param>
        /// <param name="index">The index to insert at.</param>
        public void AddIngredientAt(string ingredient, int index)
        {
            _ingredients.Insert(index, ingredient);
        }

        /// <summary>
        /// Removes an ingredient at the specified index of the _ingredients list.
        /// </summary>
        /// <param name="index">The index to remove at.</param>
        public void RemoveIngredientAt(int index)
        {
            _ingredients.RemoveAt(index);
        }

        /// <summary>
        /// Clears the _ingredients list.
        /// </summary>
        public void ClearIngredients()
        {
            _ingredients.Clear();
        }

        /// <summary>
        /// Adds a direction to end of _directions list.
        /// </summary>
        /// <param name="direction">The direction to add.</param>
        public void AddDirection(string direction)
        {
            _directions.Add(direction);
        }

        /// <summary>
        /// Adds a range of directions
        /// </summary>
        /// <param name="directions">The directions to add</param>
        public void AddDirections(IEnumerable<string> directions)
        {
            _directions.AddRange(directions);
        }

        /// <summary>
        /// Inserts a direction at the specified index of the _directions list.
        /// </summary>
        /// <param name="direction">The direction to add.</param>
        /// <param name="index">The index to insert at.</param>
        public void AddDirectionAt(string direction, int index)
        {
            _directions.Insert(index, direction);
        }

        /// <summary>
        /// Removes a direction at the specified index of the _directions list.
        /// </summary>
        /// <param name="index">The index to remove at.</param>
        public void RemoveDirectionAt(int index)
        {
            _directions.RemoveAt(index);
        }

        /// <summary>
        /// Clears the _directions list.
        /// </summary>
        public void ClearDirections()
        {
            _directions.Clear();
        }
    }
}
