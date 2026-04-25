using System.Collections.Generic;
using _Sources.LoadingStepByStep;
using UnityEngine;

public class BootstrapEntryPoint : MonoBehaviour
{
    private async void Start()
    {
        //var sdkService = new SdkService();
        
        var loadingSteps = new List<LoadingStep>()
        {
            //new LoadingStep("Загружаем SDK...", sdkService.InitializeAsync),
        };
        
        await LoadingHandler.Instance.LoadSceneAsync("SampleScene", loadingSteps);
    }
}
