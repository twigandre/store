using MediatR;
using Store.App.Crosscutting.Commom.ViewModel.Core.Usuario;
using Store.App.Crosscutting.Commom.ViewModel.Core.Filial;
using Store.App.Core.Domain.Entitites;
using Store.App.Core.Domain.Repositories;

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
            UsuarioEntity usuarioEntity = await _usuarioRepository.Selecionar(x => x.Id == request.Id, cancellationToken, "Endereco,Filial");

            if (usuarioEntity is null)
                return null;

            return new SelecionarUsuarioResponse
            {
                Id = usuarioEntity.Id,
                Nome = new UsuarioNomeVM
                {
                    PrimeiroNome = usuarioEntity.PrimeiroNome,
                    SobreNome = usuarioEntity.SobreNome
                },
                Endereco = usuarioEntity.Endereco.Count == 0 ?
                new UsuarioEnderecoVM() : 
                entityToVm(usuarioEntity.Endereco.FirstOrDefault()),
                Email = usuarioEntity.Email,
                Telefone = usuarioEntity.Telefone,
                Perfil = usuarioEntity.Perfil,
                Status = usuarioEntity.Status,
                FilialId = usuarioEntity.FilialId,
                Filial = new FilialVM
                {
                    Id = usuarioEntity.Filial.Id,
                    Nome = usuarioEntity.Filial.Nome
                }
            };
        }

        private UsuarioEnderecoVM entityToVm(UsuarioEnderecoEntity _entity) => new UsuarioEnderecoVM
        {
            Logradouro = _entity.Logradouro,
            Cidade = _entity.Cidade,
            Longitude = _entity.Longitude,
            NumeroImovel = _entity.NumeroImovel,
            Id = _entity.Id,
            Latitude = _entity.Latitude,
            ZipCode = _entity.ZipCode,
        };
    }
}
