using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.EntityFramework.DTOs
{
    public class RecipeCategoryDTO
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<RecipeDTO> Recipes { get; set; }
    }
}
