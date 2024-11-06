using System.Security.Cryptography;
using System.Text;

namespace SignalRLab3.Helper
{
    public static class PasswordHelper
    {
        private static readonly string EncryptionKey = "K1u5vdkZ1zHBC2HOk9eq5A==";

        public static string Encrypt(string clearText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                encryptor.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                encryptor.IV = new byte[16]; // AES uses a 16-byte IV by default
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                encryptor.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                encryptor.IV = new byte[16];
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    return Encoding.Unicode.GetString(ms.ToArray());
                }
            }
        }
    }

}
