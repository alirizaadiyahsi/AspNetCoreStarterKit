namespace AspNetCoreStarterKit.Application.Authentication.Dto
{
    public class ConfirmEmailInput
    {
        public string Email { get; set; }

        public string Token { get; set; }
    }
}