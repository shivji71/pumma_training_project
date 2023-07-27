using System;
using System.Security.Cryptography;
using System.Text;

public static class AesEncryption
{
    private static readonly byte[] FixedIV = Encoding.UTF8.GetBytes("YourFixedIVValue");

    public static string Encrypt(string plainText, string passphrase)
    {
        byte[] salt = Encoding.UTF8.GetBytes("YourSaltValue");

        using (PasswordDeriveBytes keyDerivationFunction = new PasswordDeriveBytes(passphrase, salt))
        {
            byte[] key = keyDerivationFunction.GetBytes(32); // 256 bits

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = FixedIV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                byte[] encryptedBytes;

                using (var memoryStream = new System.IO.MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                        cryptoStream.FlushFinalBlock();

                        encryptedBytes = memoryStream.ToArray();
                    }
                }

                byte[] ivAndEncryptedBytes = new byte[aesAlg.IV.Length + encryptedBytes.Length];
                Buffer.BlockCopy(aesAlg.IV, 0, ivAndEncryptedBytes, 0, aesAlg.IV.Length);
                Buffer.BlockCopy(encryptedBytes, 0, ivAndEncryptedBytes, aesAlg.IV.Length, encryptedBytes.Length);

                return Convert.ToBase64String(ivAndEncryptedBytes);
            }
        }
    }

    public static string Decrypt(string encryptedText, string passphrase)
    {
        byte[] ivAndEncryptedBytes = Convert.FromBase64String(encryptedText);
        byte[] salt = Encoding.UTF8.GetBytes("YourSaltValue");

        using (PasswordDeriveBytes keyDerivationFunction = new PasswordDeriveBytes(passphrase, salt))
        {
            byte[] key = keyDerivationFunction.GetBytes(32); // 256 bits

            byte[] iv = new byte[16];
            byte[] encryptedBytes = new byte[ivAndEncryptedBytes.Length - iv.Length];

            Buffer.BlockCopy(ivAndEncryptedBytes, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(ivAndEncryptedBytes, iv.Length, encryptedBytes, 0, encryptedBytes.Length);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = FixedIV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                byte[] decryptedBytes;

                using (var memoryStream = new System.IO.MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                        cryptoStream.FlushFinalBlock();

                        decryptedBytes = memoryStream.ToArray();
                    }
                }

                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }
}
