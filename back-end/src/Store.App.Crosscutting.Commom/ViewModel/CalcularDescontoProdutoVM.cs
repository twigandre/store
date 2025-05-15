namespace Store.App.Crosscutting.Commom.ViewModel
{
    public class CalcularDescontoProdutoVM
    {
        public int IdProduto { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorFinal { get; set; }
        public decimal ValorTotal { get; set; }
        public int PorcentagemDesconto { get; set; }
    }
}
