using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private GameObject _graphicsSettingsCanvas;
    [SerializeField] private GameObject _soundSettingsCanvas;

    private void Start()
    {
        GraphicsSettingsButton();
    }
    public void GraphicsSettingsButton()
    {
        _graphicsSettingsCanvas.SetActive(true);
        _soundSettingsCanvas.SetActive(false);
    }
    public void SoundSettingsButton()
    {
        _graphicsSettingsCanvas.SetActive(false);
        _soundSettingsCanvas.SetActive(true);
    }
}
