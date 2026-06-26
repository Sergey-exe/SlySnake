using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using YG; 

public class ProgressCleaner : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowBrowserConfirm(string message, string gameObjectName, string methodName);
    
    [SerializeField] private Button _cleanButton;

    private void OnEnable()
    {
        _cleanButton.onClick.AddListener(TryResetSaves);
    }

    private void OnDisable()
    {
        _cleanButton.onClick.RemoveListener(TryResetSaves);
    }

    public void TryResetSaves()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        ShowBrowserConfirm("Вы уверены? Все открытые вами уровни будут потеряны!", gameObject.name, "OnResetConfirmResult");
#else
        Debug.Log("Тест в редакторе: прогресс был бы сброшен.");
        ResetAllData();
#endif
    }

    public void OnResetConfirmResult(int result)
    {
        if (result == 1)
        {
            Debug.Log("Игрок подтвердил сброс. Очищаем данные...");
            ResetAllData();
        }
        else
        {
            Debug.Log("Игрок отменил сброс сохранений.");
        }
    }

    private void ResetAllData()
    {
        YG2.saves.CleanSaves();
        YG2.SaveProgress();

        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }
}