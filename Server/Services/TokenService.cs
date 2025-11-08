using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Server.Services
{
	public class TokenService
	{
		private const string SharedKey = "shared_secret_key";
		private readonly byte[] _key;
		private readonly byte[] _iv = new byte[16];
        private readonly string _savePath = Path.Combine(AppContext.BaseDirectory, "requests.json");

        private Dictionary<string, int> _requests = new Dictionary<string, int>();

        public TokenService()
        {
            using var sha = SHA256.Create();
            _key = sha.ComputeHash(Encoding.UTF8.GetBytes(SharedKey));
            LoadRequests();
        }
        private void LoadRequests()
        {
            if (File.Exists(_savePath))
            {
                try
                {
                    var json = File.ReadAllText(_savePath);
                    _requests = JsonSerializer.Deserialize<Dictionary<string, int>>(json) ?? new Dictionary<string, int>();
                }
                catch (Exception ex)
                {
                    _requests = new Dictionary<string, int>();
                }
            }
        }
        private void SaveRequests()
        {
            try
            {
                var json = JsonSerializer.Serialize(_requests, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_savePath, json);
            }
            catch
            {
                
            }
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
            SaveRequests();
            return _requests[userId];
        }
    }
}

