using Core.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.TestFeatures.Queries.GetTestByBookId
{
    public class GetTestByBookIdQueryResponse
    {
        public Guid TestId { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public int TotalScore { get; set; }
        public List<QuestionDto> Questions { get; set; }
    }

    public class QuestionDto
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; }
        public int Score { get; set; }
        public string Answer { get; set; }
        public List<string> Choices { get; set; }
        public string QuestionType { get; set; }
    }
}
