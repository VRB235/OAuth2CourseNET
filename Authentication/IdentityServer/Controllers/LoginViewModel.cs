namespace IdentityServer.Controllers
{
    public class LoginViewModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string returnUrl { get; set; }
    }
}