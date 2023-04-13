using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isGamePaused = false;
    
    [SerializeField] private float fixedDeltaTime = 0.02f;

    private static float _savedTimeScale;
    private static float _savedFixedDeltaTime;
    
    public static void Pause()
    {
        if (isGamePaused)
            return;
        
        isGamePaused = true;

        _savedTimeScale = Time.timeScale;
        _savedFixedDeltaTime = Time.fixedDeltaTime;
        
        Time.timeScale = 0;
        Time.fixedDeltaTime = 0;
    }

    public static void Confirm()
    {
        if (!isGamePaused)
            return;
        
        Time.timeScale = _savedTimeScale;
        Time.fixedDeltaTime = _savedFixedDeltaTime;

        isGamePaused = false;
    }

    private void Start()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = fixedDeltaTime;
    }
}
