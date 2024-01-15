namespace Irontrax.Api.Identity.Models
{
    public class IrontraxUserCreate:IrontraxUser
    {
        public string PasswordHash {  get; set; }
    }
}
