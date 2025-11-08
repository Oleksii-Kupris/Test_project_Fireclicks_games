using Domain.Network;
using Domain.Token;
using Infrastructure.Storage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Logger = Domain.Loggin.Logger;

/// <summary>
/// Handles the "Request" tab UI — sends encrypted token requests to the server
/// and displays the total number of requests for the current user.
/// </summary>
public class RequestTab : MonoBehaviour
{
  [SerializeField] private Button sendButton;
  [SerializeField] private TextMeshProUGUI resultText;
  
  private IRequestClient _requestClient;
  private ITokenService _tokenService;

  private void Awake()
  {
    _requestClient = new WebRequestClient();
    _tokenService = new TokenService(new PlayerPrefsStorage(), new AesEncryptor(SharedSecrets.EncryptionKey));
    sendButton.onClick.AddListener(OnSend);
  }

  public async void OnSend()
  {
    var enc = _tokenService.GetEncryptedToken();
    int count = await _requestClient.GetRequestCountAsync(enc);
    resultText.SetText("Requests:{0}", count);
    Logger.Log($"Requests:{count}");
  }
}
