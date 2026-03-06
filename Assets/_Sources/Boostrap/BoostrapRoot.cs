using System;
using System.Collections;
using System.Collections.Generic;
using _Sources.LoadingStepByStep;
using UnityEngine;

public class BoostrapRoot : MonoBehaviour
{
    private async void Start()
    {
        var loadingSteps = new List<LoadingStep>();
        
        await LoadingHandler.Instance.LoadSceneAsync("SampleScene", loadingSteps);
    }
}
