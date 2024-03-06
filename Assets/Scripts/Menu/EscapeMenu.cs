using System;
using LoadingSystem;
using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Menu
{
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
    
        [Header("Settings")] 
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _sfxVolumeSlider;
        [SerializeField] private Toggle _subtitlesToggle;
        [SerializeField] private Button _applySettingsButton;
        
        private static LoadingManager loadingManager => LoadingManager.I;
        
        private GameObject _activePanel;

        private void OnEnable()
        {
            _gameButton.onClick.AddListener(() => ShowPanel(_gamePanel));
            _questsButton.onClick.AddListener(() => ShowPanel(_questsPanel));
            _charactersButton.onClick.AddListener(() => ShowPanel(_charactersPanel));
            _notesButton.onClick.AddListener(() => ShowPanel(_notesPanel));
            _mapButton.onClick.AddListener(() => ShowPanel(_mapPanel));
            _settingsButton.onClick.AddListener(() => ShowPanel(_settingsPanel));
            _settingsButton.onClick.AddListener(LoadSettings);
            _continueButton.onClick.AddListener(() => gameObject.SetActive(false));
            _saveButton.onClick.AddListener(OnSaveButtonClicked);
            _loadButton.onClick.AddListener(OnLoadButtonClicked);
            _quitButton.onClick.AddListener((() => SceneManager.LoadScene(0)));
            _applySettingsButton.onClick.AddListener(OnApplySettingsButtonClicked);
            
            _activePanel = _gamePanel;
        }

        private void OnDisable()
        {
            _gameButton.onClick.RemoveAllListeners();
            _questsButton.onClick.RemoveAllListeners();
            _charactersButton.onClick.RemoveAllListeners();
            _notesButton.onClick.RemoveAllListeners();
            _mapButton.onClick.RemoveAllListeners();
            _settingsButton.onClick.RemoveAllListeners();
            _continueButton.onClick.RemoveAllListeners();
            _saveButton.onClick.RemoveAllListeners();
            _loadButton.onClick.RemoveAllListeners();
            _quitButton.onClick.RemoveAllListeners();
            _applySettingsButton.onClick.RemoveAllListeners();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ShowPanel(_gamePanel);
                gameObject.SetActive(false);
            }
        }


        private void ShowPanel(GameObject panel)
        {
            _activePanel.SetActive(false);
            panel.SetActive(true);
            _activePanel = panel;
        }
        
        private void OnSaveButtonClicked()
        {
            Debug.Log($"Saving game...");
            PlayerPrefs.SetInt("new", 0);
            
            var sceneName = gameObject.scene.name;
            
            loadingManager.DoBefore += SaveManager.SaveGame;
            loadingManager.DoAfter += SaveManager.LoadGame;
            
            loadingManager.LoadScene(sceneName);
        }
        
        private void OnLoadButtonClicked()
        {
            Debug.Log($"Loading game...");
            PlayerPrefs.SetInt("new", 0);
            
            var sceneName = gameObject.scene.name;
            
            loadingManager.DoAfter += SaveManager.LoadGame;
            
            loadingManager.LoadScene(sceneName);
        }

        private void LoadSettings()
        {
            _musicVolumeSlider.value = PlayerPrefs.GetFloat("music", 1f);
            _sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfx", 1f);
            _subtitlesToggle.isOn = PlayerPrefs.GetInt("subtitles", 1) == 1;
        }

        private void OnApplySettingsButtonClicked()
        {
            PlayerPrefs.SetFloat("music", _musicVolumeSlider.value);
            PlayerPrefs.SetFloat("sfx", _sfxVolumeSlider.value);
            PlayerPrefs.SetInt("subtitles", _subtitlesToggle.isOn ? 1 : 0);

            ShowPanel(_gamePanel);
            
            Volume.Instance.UpdateVolumes();
            Debug.Log("Settings changed");
        }
    }
}
