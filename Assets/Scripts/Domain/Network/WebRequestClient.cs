using System;
using System.Net;
using System.Threading.Tasks;
using Domain.Network;
using UnityEngine;
using UnityEngine.Networking;
using Logger = Domain.Loggin.Logger;
namespace Domain.Network
{
    /// <summary>
    /// Handles communication with the local Web API server.
    /// Sends encrypted user tokens and receives the number of requests made by that user.
    /// </summary>
    public class WebRequestClient : IRequestClient
    {
        private const string URL = "http://localhost:5119/api/request/send";
       
        public async Task<int> GetRequestCountAsync(string encryptedToken)
        {
            var json = JsonUtility.ToJson(new TokenRequest { EncryptedToken = encryptedToken });
            var body = new System.Text.UTF8Encoding().GetBytes(json);

            using var request = new UnityWebRequest(URL, "POST");
            request.uploadHandler = new UploadHandlerRaw(body);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            var op = request.SendWebRequest();
            while (!op.isDone)
                await Task.Yield();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Logger.Log($"Request failed: {request.error}");
                return -1;
            }

            var response = JsonUtility.FromJson<RequestCountJson>(request.downloadHandler.text);
            return response?.requestCount ?? -1;
        }
    }

    [Serializable]
    public class TokenRequest { public string EncryptedToken; }

    [Serializable]
    public class RequestCountJson { public int requestCount; }
}

