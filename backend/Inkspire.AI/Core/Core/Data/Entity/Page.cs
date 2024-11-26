using Core.Data.Entity.EntityBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Entity
{
    public class Page : EntityBase
    {
        public int PageNumber { get; set; } 
        public string Content { get; set; } = string.Empty;
        public Guid StoryId { get; set; }
        public Story Story { get; set; } 
    }
}
