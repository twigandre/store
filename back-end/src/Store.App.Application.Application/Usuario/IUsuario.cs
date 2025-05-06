using Store.App.Crosscutting.Commom.ViewModel.Login;
using Store.App.Crosscutting.Commom.ViewModel;

namespace Store.App.Core.Application.Usuario
{
    public interface IUsuario
    {
        RequestResponse Logar(LoginRequest obj);
    }
}
