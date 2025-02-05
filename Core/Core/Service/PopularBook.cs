using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class PopularBook
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int CommentCount { get; set; }
    }
}
