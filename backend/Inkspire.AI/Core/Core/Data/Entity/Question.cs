using Core.Data.Entity.EntityBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Entity
{
    public class Question : EntityBase
    {

        public string QuestionText { get; set; }
        public QuestionTypes QuestionType { get; set; }
        public int Score { get; set; }
        public string Answer { get; set; }
        //if type is multiple choice there will be choices but if type is word there will be no choices
        public List<string> Choices { get; set; } = new List<string>();

        //feedback
        public string Feedback { get; set; }




    }
}
