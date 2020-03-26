namespace AspNetCoreStarterKit.Application.Authentication.Dto
{
    public class LoginInput
    {
        public string UserNameOrEmail { get; set; }

        public string Password { get; set; }
    }
}
