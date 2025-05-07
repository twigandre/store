using MediatR;
using Store.App.Crosscutting.Commom.ViewModel.Core.Application;
using Store.App.Crosscutting.Commom.ViewModel.Core.Application.Usuario;
using Store.App.Crosscutting.Commom.ViewModel.Core.Application.Usuario.Listar;
using Store.App.Crosscutting.Commom.ViewModel.Pagination;
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
                        Nome = new UsuarioNome
                        {
                            PrimeiroNome = x.PrimeiroNome,
                            SobreNome = x.SobreNome
                        },
                        Endereco = x.Endereco.Count == 0 ? 
                        new UsuarioEndereco() : 
                        new UsuarioEndereco()
                        {
                            Logradouro = x.Endereco.FirstOrDefault().Logradouro,
                            Cidade = x.Endereco.FirstOrDefault().Cidade,
                            Longitude = x.Endereco.FirstOrDefault().Longitude,
                            NumeroImovel = x.Endereco.FirstOrDefault().NumeroImovel,    
                            Id = x.Endereco.FirstOrDefault().Id,    
                            Latitude = x.Endereco.FirstOrDefault().Latitude,
                            ZipCode = x.Endereco.FirstOrDefault().ZipCode,
                        },
                        Email = x.Email,
                        Telefone = x.Telefone,
                        Perfil = x.Perfil,
                        Status = x.Status,
                        FilialId = x.FilialId,
                        Filial = new Filial
                        {
                            Id = x.Filial.Id,
                            Nome = x.Filial.Nome
                        }                         
                    }).ToList()
                }
            };
        }
    }
}
