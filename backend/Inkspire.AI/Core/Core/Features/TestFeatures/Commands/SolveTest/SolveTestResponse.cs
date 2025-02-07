using Core.Data.Entity;

namespace Core.Features.TestFeatures.Commands.SolveTest
{
    public class SolveTestResponse
    {
        public Guid TestId { get; set; }
        public int TotalScore { get; set; }
        public string GeneralFeedback { get; set; }

        public List<Question> Questions { get; set; }
    }
}
