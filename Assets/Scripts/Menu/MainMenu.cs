using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _aboutPanel;

    [Header("Buttons")] 
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _loadButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _aboutButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _applySettingsButton;
    [SerializeField] private Button _closeSettingsButton;
    [SerializeField] private Button _closeAboutButton;

    private void Start()
    {
        _startButton.onClick.AddListener(()=>SceneManager.LoadScene(1));
        _loadButton.onClick.AddListener(()=>SceneManager.LoadScene(1));
        _settingsButton.onClick.AddListener(()=>_settingsPanel.SetActive(true));
        _aboutButton.onClick.AddListener(()=>_aboutPanel.SetActive(true));
        _exitButton.onClick.AddListener(Application.Quit);
        _applySettingsButton.onClick.AddListener(()=>Debug.Log("Settings changed"));
        _closeSettingsButton.onClick.AddListener(()=>_settingsPanel.SetActive(false));
        _closeAboutButton.onClick.AddListener(()=>_aboutPanel.SetActive(false));
    }   
}
