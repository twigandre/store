using Store.App.Crosscutting.Commom.Utils;
using Store.App.Crosscutting.Commom.ViewModel;

namespace Store.App.Core.Application.Produto
{
    public static class FtpProduto
    {
        public static ArquivoVM DownloadArquivo(int idProduto)
        {
            string urlUploadFtp = GeneratePathUrl.GeneratePath(Directory.GetCurrentDirectory() + "," + "Ftp" + "," + "Produto" + "," + idProduto);
            var arquivosFromFtp = Ftp.DownloadFile(urlUploadFtp).Result;
            var arquivo = arquivosFromFtp.Count == 0 ? new ArquivoVM() : arquivosFromFtp.FirstOrDefault();
            return arquivo;
        }
    }
}
