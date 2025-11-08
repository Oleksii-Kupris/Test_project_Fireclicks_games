using UnityEngine;

namespace Domain.Token
{
    public interface ITokenService
    {
        void EnsureToken();
        string GetEncryptedToken();
    }
}