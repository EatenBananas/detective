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
            loadingManager.DoAfter += SaveManager.LoadGame;
            
            loadingManager.LoadScene(SceneManager.GetSceneByBuildIndex(1).name);
        }
        
        private void OnSettingsButtonClicked()
        {
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
    }
}
