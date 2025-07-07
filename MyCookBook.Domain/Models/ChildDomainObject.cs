using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.Domain.Models
{
    public class ChildDomainObject : DomainObject
    {
        public Guid ParentId { get; set; }
    }
}
