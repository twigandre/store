namespace Store.App.Crosscutting.Utils
{
    public static class GeneratePathUrl
    {
        public static string GeneratePath(string param)
        {
            List<string> lis = new List<string>();

            if (param.Contains(","))
            {
                foreach (var item in param.Split(","))
                {
                    lis.Add(item.Trim());
                }
            }
            else
            {
                lis.Add(param);
            }

            return System.IO.Path.Combine(lis.ToArray());
        }
    }
}
