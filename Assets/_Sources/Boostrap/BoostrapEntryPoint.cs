using System.Collections.Generic;
using _Sources.LoadingStepByStep;
using UnityEngine;

public class BoostrapEntryPoint : MonoBehaviour
{
    private async void Start()
    {
        var loadingSteps = new List<LoadingStep>();
        
        await LoadingHandler.Instance.LoadSceneAsync("SampleScene", loadingSteps);
    }
}
