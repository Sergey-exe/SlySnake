/*// using System.Runtime.InteropServices;
// using System.Threading.Tasks;

public class SdkService : Service
{
    [DllImport("__Internal")]
    private static extern void InitYandexSDKJS();

    private TaskCompletionSource<bool> _sdkInitTcs;

    public override async Task InitializeAsync()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
            _sdkInitTcs = new TaskCompletionSource<bool>();
            
            // Запускаем процесс в JS
            InitYandexSDKJS();

            // Ждем, пока JS вызовет OnSDKLoaded
            await _sdkInitTcs.Task;
            
            Debug.Log("Yandex SDK успешно загружен асинхронно!");
#else
        await Task.Yield(); // Для редактора, чтобы не было ошибки
#endif
    }

    // Этот метод вызывается из JavaScript через SendMessage
    public void OnSDKLoaded()
    {
        _sdkInitTcs?.TrySetResult(true);
    }
}*/