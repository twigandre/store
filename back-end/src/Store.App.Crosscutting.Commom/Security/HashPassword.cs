using System.Security.Cryptography;
using System.Text;

namespace Store.App.Crosscutting.Commom.Security
{
    public static class HashPassword
    {
        public static string StringToHash(string input)
        {
            SHA512 sha256Hash = SHA512.Create();
            string hash = GetHash(sha256Hash, input);
            return hash;
        }

        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            var sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

    }
}
