using Domain.Token;
using Infrastructure.Encryption;
using Infrastructure.Storage;
using UnityEngine;

namespace Domain.Token
{
    /// <summary>
    /// Manages the lifecycle of the client authentication token.  
    /// Responsible for generating, storing, and encrypting the token used in API requests.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly IStorage _storage;
        private readonly IEncryptor _encryptor;

        private const string TOKEN_KEY = "token_key";

        public TokenService(IStorage storage, IEncryptor encryptor)
        {
            _storage = storage;
            _encryptor = encryptor;
        }

        public void EnsureToken()
        {
            if (!_storage.HasKey(TOKEN_KEY))
            {
                string raw = System.Guid.NewGuid().ToString("N");
                _storage.SetValue(TOKEN_KEY, raw);
            }
        }

        public string GetEncryptedToken()
        {
            string raw = _storage.GetValue(TOKEN_KEY, string.Empty);
            if (string.IsNullOrEmpty(raw))
            {
                EnsureToken();
                raw = _storage.GetValue(TOKEN_KEY, string.Empty);
            }

            return _encryptor.Encrypt(raw);
        }
    }
}