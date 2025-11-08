using System.Security.Cryptography;
using System.Text;

namespace Server.Services
{
	public class TokenService
	{
		private const string SharedKey = "shared_secret_key";
		private readonly byte[] _key;
		private readonly byte[] _iv = new byte[16];
		private readonly Dictionary<string, int> _requests = new Dictionary<string, int>();

        public TokenService()
        {
            using var sha = SHA256.Create();
            _key = sha.ComputeHash(Encoding.UTF8.GetBytes(SharedKey));
        }

        public string? DecryptToken(string encrypted)
        {
            try
            {
                byte[] data = Convert.FromBase64String(encrypted);

                using var aes = Aes.Create();
                aes.Key = _key;
                aes.IV = _iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using var decryptor = aes.CreateDecryptor();
                byte[] plain = decryptor.TransformFinalBlock(data, 0, data.Length);

                return System.Text.Encoding.UTF8.GetString(plain);
            }
            catch
            {
                return null;
            }
        }

        public int RegisterRequest(string userId)
        {
            if (!_requests.ContainsKey(userId))
                _requests[userId] = 0;

            _requests[userId]++;
            return _requests[userId];
        }
    }
}

