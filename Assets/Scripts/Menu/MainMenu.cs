using System;
using System.IO;
using LoadingSystem;
using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Menu
{
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

        [Header("Settings")] 
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _sfxVolumeSlider;
        [SerializeField] private Toggle _subtitlesToggle;
        
        private static LoadingManager loadingManager => LoadingManager.I;
        private static bool isSaveExists => Directory.GetFiles(SaveManager.SaveFolderPath).Length > 0;

        private void OnEnable()
        {
            _startButton.onClick.AddListener(OnStartButtonClicked);
            _loadButton.onClick.AddListener(OnLoadButtonClicked);
            _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
            _aboutButton.onClick.AddListener(OnAboutButtonClicked);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
            _applySettingsButton.onClick.AddListener(OnApplySettingsButtonClicked);
            _closeSettingsButton.onClick.AddListener(OnCloseSettingsButtonClicked);
            _closeAboutButton.onClick.AddListener(OnCloseAboutButtonClicked);
        }

        private void Start()
        {
            UpdateUI();
        }

        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(OnStartButtonClicked);
            _loadButton.onClick.RemoveListener(OnLoadButtonClicked);
            _settingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
            _aboutButton.onClick.RemoveListener(OnAboutButtonClicked);
            _exitButton.onClick.RemoveListener(OnExitButtonClicked);
            _applySettingsButton.onClick.RemoveListener(OnApplySettingsButtonClicked);
            _closeSettingsButton.onClick.RemoveListener(OnCloseSettingsButtonClicked);
            _closeAboutButton.onClick.RemoveListener(OnCloseAboutButtonClicked);
        }

        private void UpdateUI()
        {
            _loadButton.interactable = isSaveExists;
        }

        private void OnStartButtonClicked()
        {
            PlayerPrefs.SetInt("new", 1);
            
            loadingManager.DoBefore += () =>
            {
                if (isSaveExists)
                    File.Delete(SaveManager.SaveFilePath);
            };
            
            loadingManager.DoAfter += SaveManager.SaveGame;
            
            loadingManager.LoadScene("_Main");
        }

        private void OnLoadButtonClicked()
        {
            PlayerPrefs.SetInt("new", 0);
            
            loadingManager.DoAfter += SaveManager.LoadGame;
            
            loadingManager.LoadScene("_Main");
        }
        
        private void OnSettingsButtonClicked()
        {
            LoadSettings();
            _settingsPanel.SetActive(true);
        }
        
        private void OnAboutButtonClicked()
        {
            _aboutPanel.SetActive(true);
        }
        
        private void OnExitButtonClicked()
        {
            Application.Quit();
        }
        
        private void OnApplySettingsButtonClicked()
        {
            PlayerPrefs.SetFloat("music", _musicVolumeSlider.value);
            PlayerPrefs.SetFloat("sfx", _sfxVolumeSlider.value);
            PlayerPrefs.SetInt("subtitles", _subtitlesToggle.isOn ? 1 : 0);
            
            Debug.Log("Settings changed");
        }
        
        private void OnCloseSettingsButtonClicked()
        {
            _settingsPanel.SetActive(false);
        }
        
        private void OnCloseAboutButtonClicked()
        {
            _aboutPanel.SetActive(false);
        }

        private void LoadSettings()
        {
            _musicVolumeSlider.value = PlayerPrefs.GetFloat("music", 1f);
            _sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfx", 1f);
            _subtitlesToggle.isOn = PlayerPrefs.GetInt("subtitles", 1) == 1;
        }
    }
}
