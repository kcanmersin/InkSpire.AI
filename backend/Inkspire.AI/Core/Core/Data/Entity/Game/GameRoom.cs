using Core.Data.Entity.User;

namespace Core.Data.Entity.Game
{
    public class GameRoom
    {
        public Guid RoomId { get; set; } = Guid.NewGuid();
        public string Language { get; set; }
        public List<AppUser> Players { get; set; } = new List<AppUser>();
        public bool IsGameStarted => Players.Count == 2;
        public string? Topic { get; set; }
        public int Player1Score { get; set; } = 0;
        public int Player2Score { get; set; } = 0;
        public Guid CurrentTurn { get; set; }
        public List<string> UsedAnswers { get; set; } = new List<string>();
    }
}
