using System;
using UnityEngine;
using UnityEngine.Serialization;

public class TabsController : MonoBehaviour
{
 [SerializeField] private GameObject big, grouped, request;

  public void ShowBigTab() => Show(big);
  public void ShowGroupedTab() => Show(grouped);
  public void ShowRequestTab() => Show(request);

  private void Start()
  {
      Show(big);
  }

  private void Show(GameObject target)
  {
    big.SetActive(target == big);
    grouped.SetActive(target == grouped);
    request.SetActive(target == request);
  }
}
