using Store.App.Crosscutting.Commom.ViewModel;
using System.Collections.Generic;

namespace Store.App.Crosscutting.Commom.Utils
{
    public static class Ftp
    {

        public static RequestResponseVM UploadFile(string path,
                                                   string hashFile,
                                                   string fileName)
        {
            DirectoryInfo di = new DirectoryInfo(GeneratePathUrl.GeneratePath(path));

            if (Directory.Exists(GeneratePathUrl.GeneratePath(path)))
            {
                foreach (FileInfo file in di?.GetFiles())
                {
                    file.Delete();
                }
            }
            else
            {
                Directory.CreateDirectory(path);
            }

            var arrayBytesPdf = Convert.FromBase64String(hashFile);

            File.WriteAllBytes(fileName, arrayBytesPdf);

            string arquivoOrigem = GeneratePathUrl.GeneratePath(Directory.GetCurrentDirectory() + "," + fileName);

            string arquivoDestino = GeneratePathUrl.GeneratePath(path + "," + fileName);

            File.Copy(arquivoOrigem, arquivoDestino);

            File.Delete(arquivoOrigem);

            return new RequestResponseVM();
        }

        public static async Task<List<ArquivoVM>> DownloadFile(string path)
        {
            byte[] files;

            string diretorioDestino = GeneratePathUrl.GeneratePath(path);

            bool temArquivos = Directory.Exists(diretorioDestino) &&
                               Directory.GetFiles(diretorioDestino).Length > 0;

            List<ArquivoVM> retornoMetodo = new();

            if (temArquivos)
            {
                string[] arquivos = Directory.GetFiles(diretorioDestino);

                foreach (var arquivo in arquivos)
                {
                    string diretorioArquivo = GeneratePathUrl.GeneratePath(diretorioDestino + "," + arquivo);
                    
                    byte[] file = await System.IO.File.ReadAllBytesAsync(diretorioArquivo);
                    
                    string nomeArquivo = Path.GetFileName(arquivo);

                    retornoMetodo.Add(new ArquivoVM { 
                        Nome = nomeArquivo,
                        Hash = Convert.ToBase64String(file)
                    });
                }
            }

            return retornoMetodo;
        }

        public static RequestResponseVM DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo di = new DirectoryInfo(GeneratePathUrl.GeneratePath(path));

                if (Directory.Exists(GeneratePathUrl.GeneratePath(path)))
                {
                    foreach (FileInfo file in di?.GetFiles())
                    {
                        file.Delete();
                    }
                }

                Directory.Delete(path, true);

                return new RequestResponseVM();
            }
            else
            {
                return new RequestResponseVM
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    TextResponse = "FILE NOT FOUND - remove operation"
                };
            }
        }
    }
}
