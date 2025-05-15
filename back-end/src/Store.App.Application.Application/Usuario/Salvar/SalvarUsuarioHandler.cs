using MediatR;
using Store.App.Crosscutting.Commom.Security;
using Store.App.Crosscutting.Commom.ViewModel.Core.Application.Usuario.Salvar;
using Store.App.Infrastructure.Database.DbEntities;
using Store.App.Infrastructure.Database.DbRepository.Filial;
using Store.App.Infrastructure.Database.DbRepository.Usuario;
using Store.App.Insfrastructure.Database.DbRepository.Usuario.UsuarioEndereco;
using System.Net;

namespace Store.App.Core.Application.Usuario.Salvar
{
    public class SalvarUsuarioHandler : IRequestHandler<SalvarUsuarioCommand, SalvarUsuarioResponse>
    {
        IUsuarioRepository _repositoryUsuario;
        IUsuarioEnderecoRepository _repositoryUsuarioEndereco;
        IFilialRepository _filialRepository;
        public SalvarUsuarioHandler(IUsuarioRepository repository,
                                    IUsuarioEnderecoRepository repositoryUsuarioEndereco,
                                    IFilialRepository filialRepository)
        {
            _repositoryUsuario = repository;
            _repositoryUsuarioEndereco = repositoryUsuarioEndereco;
            _filialRepository = filialRepository;
        }
        public async Task<SalvarUsuarioResponse> Handle(SalvarUsuarioCommand request, CancellationToken cancellationToken)
        {
            UsuarioEntity entity = new();

            bool isNovoUsuario = (request.Id is null || request.Id == 0);

            int idEndereco = 0;

            bool filialExiste = await _filialRepository.Existe(x => x.Id == request.FilialId, cancellationToken);

            if (!filialExiste)
            {
                return new SalvarUsuarioResponse()
                {
                    StatusCode = HttpStatusCode.Conflict,
                    TextResponse = "Filial informada não existe!"
                };
            }

            if (isNovoUsuario)
            {
                if (string.IsNullOrEmpty(request.Senha))
                {
                    return new SalvarUsuarioResponse()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        TextResponse = "Senha não informada. (Se for um novo usuário, será necessário informada a senha.)"
                    };
                }

                bool emailOuTelefoneJaCadastrado = await _repositoryUsuario.Existe(x => x.Email.ToUpper().Equals(request.Email.ToUpper()) ||
                                                                                        x.Telefone.ToUpper().Equals(request.Telefone.ToUpper()), cancellationToken);

                if (emailOuTelefoneJaCadastrado)
                {
                    return new SalvarUsuarioResponse()
                    {
                        StatusCode = HttpStatusCode.Conflict,
                        TextResponse = "E-mail ou Telefone já cadastrado."
                    };
                }

                //gera a senha do usuário
                entity.Senha = HashPassword.StringToHash(request.Senha);
                entity.Id = 0;
            }
            else
            {
                entity = await _repositoryUsuario.Selecionar(x => x.Id == request.Id, cancellationToken, "Endereco");
                
                entity.Id = (int) request.Id;
                idEndereco = entity.Endereco.FirstOrDefault().Id;
            }

            entity.PrimeiroNome = request.Nome.PrimeiroNome;
            entity.SobreNome = request.Nome.SobreNome;
            entity.Email = request.Email;
            entity.Telefone = request.Telefone;
            entity.Perfil = request.Perfil;
            entity.FilialId = request.FilialId;
            entity.Status = request.Status;

            _repositoryUsuario.Salvar(entity);

            await _repositoryUsuario.Context.SaveChangesAsync(cancellationToken);

            var endereco = new UsuarioEnderecoEntity
            {
                Cidade = request.Endereco.Cidade,
                Logradouro = request.Endereco.Logradouro,
                NumeroImovel = request.Endereco.NumeroImovel,
                ZipCode = request.Endereco.ZipCode,
                Latitude = request.Endereco.Latitude,
                Longitude = request.Endereco.Longitude,
                Id = idEndereco,
                UsuarioId = entity.Id
            };

            await _repositoryUsuarioEndereco.SalvarEnderecoUsuario(endereco, cancellationToken);

            return new SalvarUsuarioResponse
            {
                Id = entity.Id,
                StatusCode = isNovoUsuario ? HttpStatusCode.Created : HttpStatusCode.OK,
                TextResponse = "Usuário " + ((isNovoUsuario)? "inserido" : "atualizado") + " com sucesso."
            };
        }
    }
}
