using Core.Data.Entity.EntityBases;

namespace Core.Data.Entity.User
{
    public class Word : EntityBase
    {
        public Guid UserId { get; set; }
        public string WordText { get; set; }
        public string TranslatedText { get; set; }

        public List<string> Examples { get; set; } = new();  
        public List<string> ExampleDescriptions { get; set; } = new();  
    }


}