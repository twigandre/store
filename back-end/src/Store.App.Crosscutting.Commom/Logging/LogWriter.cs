using Store.App.Crosscutting.Commom.Utils;

namespace Store.App.Crosscutting.Commom.Logging
{
    public static class LogWriter
    {
        public static string GenerateDefaultLog(string logMessage, bool isSalesLog = false)
        {
            string urlDirectory = Directory.GetCurrentDirectory() + "," + 
                                 "ApplicationLog," + 
                                 (isSalesLog ? "Sales," : "") + 
                                 "DefaultLog";

            urlDirectory = GeneratePathUrl.GeneratePath(urlDirectory);

            bool fileExists = Directory.Exists(GeneratePathUrl.GeneratePath(urlDirectory));

            if (!fileExists)
            {
                Directory.CreateDirectory(urlDirectory);
            }

            try
            {
                string[] lines = { "\r\nLog Entry :\n" + DateTime.Now.ToLongTimeString() + "\n\n" + logMessage + "\n-------------------------------" };

                string fileName = isSalesLog ? "SalesLog" : "DefaultLog";

                File.AppendAllLines(urlDirectory + "/" + fileName + DateTime.Now.ToString("ddMMyyyy") + "-log.txt", lines);

                return "log gerado corretamente";
            }
            catch (Exception ex)
            {
                return "falha ao gerar log " + ex.Message + ex.InnerException;
            }
        }
    }
}
