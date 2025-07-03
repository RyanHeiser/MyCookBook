using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.Domain.Models
{
    public class RecipeImage
    {
        [Key]
        public Guid RecipeId { get; set; }
        public byte[]? RawImageData { get; set; }

        public RecipeImage(Guid recipeId, byte[] rawImageData)
        {
            RecipeId = recipeId;
            RawImageData = rawImageData;
        }
    }
}
