using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.EntityFramework.DTOs
{
    public class RecipeDTO
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Minutes { get; set; }
        public int Servings { get; set; }
        public ICollection<string>? Ingredients { get; set; }
        public ICollection<string>? Directions { get; set; }
    }
}
