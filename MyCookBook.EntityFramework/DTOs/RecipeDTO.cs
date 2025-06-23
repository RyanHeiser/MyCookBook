using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.EntityFramework.DTOs
{
    public class RecipeDTO : DTO
    {
        public string Name { get; set; }
        public int Minutes { get; set; }
        public int Servings { get; set; }
        public required List<string> Ingredients { get; set; }
        public required List<string> Directions { get; set; }
    }
}
