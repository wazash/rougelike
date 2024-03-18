using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NewSaveSystem
{
    public static class EncryptionHelper
    {
        private static readonly byte[] key = Encoding.UTF8.GetBytes("Kt&jCpTAMZdYYg^F@zQWhZYvDgP3EYx$");
        private static readonly byte[] iv = Encoding.UTF8.GetBytes("kQPQ52a4^KBs9hos");

        public static string EncryptString(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            using var sw = new StreamWriter(cs);
            sw.Write(plainText);

            return System.Convert.ToBase64String(ms.ToArray());
        }

        public static string DecryptString(string cipherText)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(System.Convert.FromBase64String(cipherText));
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
    }
}
