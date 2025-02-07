namespace API.Contracts
{
    public class CreateWordRequest
    {
        public string WordText { get; set; }
        public string TranslatedText { get; set; }
        public Dictionary<string, string> Examples { get; set; } = new();
    }
}