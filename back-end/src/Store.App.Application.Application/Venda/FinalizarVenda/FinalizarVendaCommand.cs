using MediatR;
using Store.App.Core.Application.Carro.Listar;
using System.ComponentModel.DataAnnotations;

namespace Store.App.Core.Application.Venda.FinalizarVenda
{
    public class FinalizarVendaCommand : IRequest<FinalizarVendaResponse>
    {
        public int IdCarro { get; set; }
    }
}
