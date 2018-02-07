namespace MLaw.Idp.Mvc.Models
{
    public class IdentityServerRequestModel
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Code { get; set; }
    }
}
