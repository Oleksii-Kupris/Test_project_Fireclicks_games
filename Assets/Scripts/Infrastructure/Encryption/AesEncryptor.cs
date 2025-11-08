using System;
using System.Security.Cryptography;
using System.Text;
using Infrastructure.Encryption;

/// <summary>
/// AES-based encryption utility implementing <see cref="IEncryptor"/>.
/// Uses CBC mode with PKCS7 padding and a shared key hashed via SHA-256.
/// </summary>
public class AesEncryptor : IEncryptor
{
    private readonly byte[] _key;
    private readonly byte[] _iv = new byte[16];

    public AesEncryptor(string sharedKey)
    {
        using (var sha = SHA256.Create())
        {
            _key = sha.ComputeHash(Encoding.UTF8.GetBytes(sharedKey));
        }
    }

    public string Encrypt(string plainText)
    {
       using var aes = Aes.Create();
       aes.Key = _key;
       aes.IV = _iv;
       aes.Mode = CipherMode.CBC;
       aes.Padding = PaddingMode.PKCS7;
       
       var data = Encoding.UTF8.GetBytes(plainText);
       using var encryptor = aes.CreateEncryptor();
       var encryptedData = encryptor.TransformFinalBlock(data, 0, data.Length);
       
       return Convert.ToBase64String(encryptedData);
    }
}
