using System;
using System.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LoadingSystem
{
    public class LoadingManager : MonoBehaviour
    {
        public static LoadingManager I { get; private set; }
        
        public event Action DoBefore;
        public event Action DoOnLoading;
        public event Action DoAfter;

        private const int MIN_LOADING_TIME = 3000;
        private const int PROGRESS_CHECK_DELAY = 100;
        
        [SerializeField] private string _loadingScreenSceneName = "LoadingScreen";
        [SerializeField] private Canvas _coverCanvas;
        [SerializeField] private Image _coverImage;

        private void Awake()
        {
            if (I != null)
            {
                Destroy(gameObject);
                return;
            }

            I = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }

        [Button]
        public async void LoadScene(string sceneName)
        {
            var currentSceneName = SceneManager.GetActiveScene().name;

            await ShowCover(_coverImage);
            await AddScene(_loadingScreenSceneName);
            await HideCover(_coverImage);

            WaitForAction(DoBefore);

            await RemoveScene(currentSceneName);

            WaitForAction(DoOnLoading);

            await AddScene(sceneName);

            WaitForAction(DoAfter);

            DoBefore = null;
            DoOnLoading = null;
            DoAfter = null;

            LightProbes.TetrahedralizeAsync();

            await ShowCover(_coverImage);
            await RemoveScene(_loadingScreenSceneName);
            await HideCover(_coverImage);
        }

        private static async Task AddScene(string sceneName)
        {
            var scene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            scene.allowSceneActivation = false;

            while (scene.progress < 0.9f) 
                await Task.Delay(PROGRESS_CHECK_DELAY);

            scene.allowSceneActivation = true;            

            while (!scene.isDone)
                await Task.Delay(PROGRESS_CHECK_DELAY);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
		}

        private static async Task RemoveScene(string sceneName)
        {
            var scene = SceneManager.UnloadSceneAsync(sceneName);

            while (scene.progress < 0.9f)
                await Task.Delay(PROGRESS_CHECK_DELAY);

            while (!scene.isDone)
                await Task.Delay(PROGRESS_CHECK_DELAY);
        }

        private static void WaitForAction(Action action)
        {
            if (action == null) return;
            
            var task = new Task(action);
            task.Start(TaskScheduler.FromCurrentSynchronizationContext());
            task.Wait();
        }

        private async Task ShowCover(Image image)
        {
            _coverCanvas.gameObject.SetActive(true);
            
            await image.DOFade(1, 1).SetUpdate(true).AsyncWaitForCompletion();
            
            await Task.CompletedTask;
        }

        private async Task HideCover(Image image)
        {
            await image.DOFade(0, 1).SetUpdate(true).AsyncWaitForCompletion();
            
            _coverCanvas.gameObject.SetActive(false);
            
            await Task.CompletedTask;
        }
    }
}
