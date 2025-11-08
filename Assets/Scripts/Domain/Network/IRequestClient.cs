using System.Threading.Tasks;
using UnityEngine;

namespace Domain.Network
{
    public interface IRequestClient
    {
        Task<int> GetRequestCountAsync(string encryptedToken);
    }
}