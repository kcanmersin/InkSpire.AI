using Core.Data.Entity.EntityBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Entity
{
    public class StoryImage : EntityBase
    {
        public Guid StoryId { get; set; }
        public Story Story { get; set; }

        public byte[] ImageData { get; set; }

        //order
        public int Page { get; set; }

    }
}
