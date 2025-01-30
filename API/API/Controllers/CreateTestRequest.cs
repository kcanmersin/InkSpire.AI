using System.ComponentModel;

namespace API.Controllers
{
    public class CreateTestRequest
    {

        [System.ComponentModel.DefaultValue("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7")]
        public Guid UserId { get; set; }
        [System.ComponentModel.DefaultValue("6b13c129-8a4c-4511-bf7b-53704f060bdb")]

        public Guid BookId { get; set; }
    }
}
