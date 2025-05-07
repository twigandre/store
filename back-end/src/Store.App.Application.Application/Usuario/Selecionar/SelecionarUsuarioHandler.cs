using MediatR;
using Store.App.Crosscutting.Commom.ViewModel.Core.Application.Usuario;
using Store.App.Crosscutting.Commom.ViewModel.Core.Application;
using Store.App.Infrastructure.Database.DbEntities;
using Store.App.Infrastructure.Database.DbRepository.Usuario;

namespace Store.App.Core.Application.Usuario.Selecionar
{
    public class SelecionarUsuarioHandler : IRequestHandler<SelecionarUsuarioCommand, SelecionarUsuarioResponse>
    {
        IUsuarioRepository _usuarioRepository;
        public SelecionarUsuarioHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<SelecionarUsuarioResponse> Handle(SelecionarUsuarioCommand request, CancellationToken cancellationToken)
        {
            UsuarioEntity usuarioEntity = await _usuarioRepository.Selecionar(x => x.Id == request.Id, "Endereco,Filial", cancellationToken);

            if (usuarioEntity is null)
                return null;

            return new SelecionarUsuarioResponse
            {
                Id = usuarioEntity.Id,
                Nome = new UsuarioNome
                {
                    PrimeiroNome = usuarioEntity.PrimeiroNome,
                    SobreNome = usuarioEntity.SobreNome
                },
                Endereco = usuarioEntity.Endereco.Count == 0 ?
                new UsuarioEndereco() :
                new UsuarioEndereco()
                {
                    Logradouro = usuarioEntity.Endereco.FirstOrDefault().Logradouro,
                    Cidade = usuarioEntity.Endereco.FirstOrDefault().Cidade,
                    Longitude = usuarioEntity.Endereco.FirstOrDefault().Longitude,
                    NumeroImovel = usuarioEntity.Endereco.FirstOrDefault().NumeroImovel,
                    Id = usuarioEntity.Endereco.FirstOrDefault().Id,
                    Latitude = usuarioEntity.Endereco.FirstOrDefault().Latitude,
                    ZipCode = usuarioEntity.Endereco.FirstOrDefault().ZipCode,
                },
                Email = usuarioEntity.Email,
                Telefone = usuarioEntity.Telefone,
                Perfil = usuarioEntity.Perfil,
                Status = usuarioEntity.Status,
                FilialId = usuarioEntity.FilialId,
                Filial = new Filial
                {
                    Id = usuarioEntity.Filial.Id,
                    Nome = usuarioEntity.Filial.Nome
                }
            };
        }
    }
}
