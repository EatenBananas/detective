using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private GameObject _questsPanel;
    [SerializeField] private GameObject _charactersPanel;
    [SerializeField] private GameObject _notesPanel;
    [SerializeField] private GameObject _mapPanel;
    [SerializeField] private GameObject _settingsPanel;
    [Header("Panel Buttons")]
    [SerializeField] private Button _gameButton;
    [SerializeField] private Button _questsButton;
    [SerializeField] private Button _charactersButton;
    [SerializeField] private Button _notesButton;
    [SerializeField] private Button _mapButton;
    [Header("GamePanelButtons")]
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _loadButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;
    
    private GameObject _activePanel;

    private void Start()
    {
        _activePanel = _gamePanel;
        
        _gameButton.onClick.AddListener(()=>ShowPanel(_gamePanel));
        _questsButton.onClick.AddListener(()=>ShowPanel(_questsPanel));
        _charactersButton.onClick.AddListener(()=>ShowPanel(_charactersPanel));
        _notesButton.onClick.AddListener(()=>ShowPanel(_notesPanel));
        _mapButton.onClick.AddListener(()=>ShowPanel(_mapPanel));
        _settingsButton.onClick.AddListener(()=>ShowPanel(_settingsPanel));
        
        _continueButton.onClick.AddListener(()=>gameObject.SetActive(false));
        _quitButton.onClick.AddListener((()=>SceneManager.LoadScene(0)));
    }

    private void ShowPanel(GameObject panel)
    {
        _activePanel.SetActive(false);
        panel.SetActive(true);
        _activePanel = panel;
    }
    
    
}
