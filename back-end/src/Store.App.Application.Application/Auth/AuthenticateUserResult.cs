using Store.App.Crosscutting.Commom.ViewModel;
namespace Store.App.Core.Application.Auth
{
    public class AuthenticateUserResult : RequestResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}
