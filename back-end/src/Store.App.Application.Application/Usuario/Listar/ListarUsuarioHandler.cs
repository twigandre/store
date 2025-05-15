using MediatR;
using Store.App.Crosscutting.Commom.ViewModel.Core.Filial;
using Store.App.Crosscutting.Commom.ViewModel.Core.Usuario;
using Store.App.Crosscutting.Commom.ViewModel.Core.Usuario.Listar;
using Store.App.Crosscutting.Commom.ViewModel.Pagination;
using Store.App.Infrastructure.Database.DbEntities;
using Store.App.Infrastructure.Database.DbRepository.Usuario;

namespace Store.App.Core.Application.Usuario.Listar
{
    public class ListarUsuarioHandler : IRequestHandler<ListarUsuarioCommand, ListarUsuarioResponse>
    {
        IUsuarioRepository _repository;
        public ListarUsuarioHandler(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<ListarUsuarioResponse> Handle(ListarUsuarioCommand request, CancellationToken cancellationToken)
        {
            var filter = (request.obj is null) ? new ListarUsuarioRequest() : request.obj;

            var resultadoPaginado = await _repository.ListarUsuariosPaginado(filter, cancellationToken);

            if (resultadoPaginado.Total == 0)
            {
                return new ListarUsuarioResponse
                { 
                    data = new PagedItems<ListarUsuarioResult>
                    {
                        Total = 0,
                        Items = null
                    }
                };
            }

            return new ListarUsuarioResponse
            {
                data = new PagedItems<ListarUsuarioResult>
                {
                    Total = resultadoPaginado.Total,
                    Items = resultadoPaginado.Items.Select(x => new ListarUsuarioResult
                    {
                        Id = x.Id,
                        Nome = new UsuarioNomeVM
                        {
                            PrimeiroNome = x.PrimeiroNome,
                            SobreNome = x.SobreNome
                        },
                        Endereco = entityToVm(x.Endereco.FirstOrDefault()),
                        Email = x.Email,
                        Telefone = x.Telefone,
                        Perfil = x.Perfil,
                        Status = x.Status,
                        FilialId = x.FilialId,
                        Filial = new FilialVM
                        {
                            Id = x.Filial.Id,
                            Nome = x.Filial.Nome
                        }
                    }).ToList()
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
