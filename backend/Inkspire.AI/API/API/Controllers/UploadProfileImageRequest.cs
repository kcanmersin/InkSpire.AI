namespace API.Controllers
{
    public class UploadProfileImageRequest
    {
        [DefaultValue("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7")]
        public Guid UserId { get; set; }
        public IFormFile File { get; set; }
    }

}
