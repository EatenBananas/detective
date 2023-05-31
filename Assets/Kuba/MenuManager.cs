using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuCanvas;
    [SerializeField] private GameObject _settingsCanvas;
    [SerializeField] private GameObject _creatingGameCanvas;

    private void Start()
    {
        BackToMainMenuButton();
    }
    public void StartGameButton()
    {
        //load new scene
        Debug.Log("What Game?");
        CreateGameBtn();
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
        _creatingGameCanvas.SetActive(false);
    }
    public void BackToMainMenuButton()
    {
        _settingsCanvas.SetActive(false);
        _mainMenuCanvas.SetActive(true);
        _creatingGameCanvas.SetActive(false);
    }
    public void CreateGameBtn()
    {
        _settingsCanvas.SetActive(false);
        _mainMenuCanvas.SetActive(true);
        _creatingGameCanvas.SetActive(true);
    }
}
