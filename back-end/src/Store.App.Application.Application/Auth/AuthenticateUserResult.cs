using Store.App.Crosscutting.Commom.ViewModel;
using Store.App.Crosscutting.Commom.ViewModel.Login;

namespace Store.App.Core.Application.Auth
{
    public class AuthenticateUserResult : RequestResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}
