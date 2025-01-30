using Core.Data.Entity.EntityBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Entity
{
    public class Test : EntityBase
    {
        //question list
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();



        //public score
        public int TotalScore { get; set; }

        public string GeneralFeedback { get; set; }

        //bookId
        public Guid BookId { get; set; }
        //userId
        public Guid UserId { get; set; }





    }
}
