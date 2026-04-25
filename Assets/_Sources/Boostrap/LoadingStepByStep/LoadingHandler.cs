using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Sources.LoadingStepByStep
{
    public class LoadingHandler : MonoBehaviour
    {
        public static LoadingHandler Instance { get; private set; }
    
        [SerializeField] private GameObject _loadingUI;
        [SerializeField] private Slider _progressBar;
        [SerializeField] private TMP_Text _statusText;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public async Task LoadSceneAsync(string sceneName, List<LoadingStep>  loadingSteps)
        {
            var loadingSceneStep = new LoadingStep("Создаём виртуальный мир...", async () => await LoadScene(sceneName));
            loadingSteps.Add(loadingSceneStep);
            
            var stepFraction = 1f / loadingSteps.Count;
            var totalProgress = 0f;
            
            _progressBar.value = totalProgress;
            _loadingUI.SetActive(true);

            foreach (var step in loadingSteps)
            {
                _statusText.text = step.Description;
                await step.ActionAsync();
                
                totalProgress += stepFraction;
                _progressBar.value = totalProgress;
            }
            
            _loadingUI.SetActive(false);
        }

        private static async Task LoadScene(string sceneName)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;

            while (asyncOperation.progress < 0.9f)
                await Task.Yield();
            
            asyncOperation.allowSceneActivation = true;
        }
    }
}
