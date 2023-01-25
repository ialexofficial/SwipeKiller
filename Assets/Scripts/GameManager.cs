using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isGamePaused = false;
    
    [SerializeField] private float fixedDeltaTime = 0.02f;

    private void Start()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = fixedDeltaTime;
    }
}
