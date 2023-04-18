using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuCanvas;
    [SerializeField] private GameObject _settingsCanvas;

    private void Start()
    {
        BackToMainMenuButton();
    }
    public void StartGameButton()
    {
        //load new scene
        Debug.Log("StartNewGame");
    }

    public void QuitGameButton()
    {
        //Application.Quit();
        Debug.Log("alt+f4 mate");
    }

    public void SettingsButton()
    {
        _settingsCanvas.SetActive(true);
        _mainMenuCanvas.SetActive(false);
    }
    public void BackToMainMenuButton()
    {
        _settingsCanvas.SetActive(false);
        _mainMenuCanvas.SetActive(true);
    }
}
