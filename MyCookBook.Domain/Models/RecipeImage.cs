using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.Domain.Models
{
    public class RecipeImage : ChildDomainObject
    {
        public byte[]? RawImageData { get; set; }

        public RecipeImage(byte[] rawImageData)
        {
            Id = Guid.NewGuid();
            RawImageData = rawImageData;
        }

        public RecipeImage(byte[] rawImageData, Guid recipeId)
        {
            Id = Guid.NewGuid();
            ParentId = recipeId;
            RawImageData = rawImageData;
        }
    }
}
