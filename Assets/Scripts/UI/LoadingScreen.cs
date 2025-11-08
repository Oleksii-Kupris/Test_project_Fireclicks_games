using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI loadingText;

    public void SetProgress(float progress)
    {
        progressBar.value = progress;
        loadingText.SetText($"Loading: {progress:P0}");
    }
}