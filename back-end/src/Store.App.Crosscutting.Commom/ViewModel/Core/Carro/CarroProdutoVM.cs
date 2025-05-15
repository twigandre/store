namespace Store.App.Crosscutting.Commom.ViewModel.Core.Carro
{
    public class CarroProdutoVM
    {
        public int Id { get; set; }

        public int CarroId { get; set; }

        public int ProdutoId { get; set; }

        public int Quantidade { get; set; }

        public DateTime DataHoraUltimaAlteracao { get; set; }
    }
}
