using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool IsGamePaused = false;
    
    [SerializeField] private float fixedDeltaTime = 0.02f;

    private static float _savedTimeScale;
    private static float _savedFixedDeltaTime;
    
    public static void Pause()
    {
        if (IsGamePaused)
            return;
        
        IsGamePaused = true;

        _savedTimeScale = Time.timeScale;
        _savedFixedDeltaTime = Time.fixedDeltaTime;
        
        Time.timeScale = 0;
        Time.fixedDeltaTime = 0;
    }

    public static void Confirm()
    {
        if (!IsGamePaused)
            return;
        
        Time.timeScale = _savedTimeScale;
        Time.fixedDeltaTime = _savedFixedDeltaTime;

        IsGamePaused = false;
    }

    private void Start()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = fixedDeltaTime;
        Application.targetFrameRate = 200;
    }
}
