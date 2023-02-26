using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public static GameManager Instance { get; private set; }

    [Header("Screen Operations")]
    [SerializeField] private int RefreshRate = 60;
    private void Awake()
    {
        CheckInstance();
        SetRefreshRateAccordingToDevice();
    }
    private void Start()
    {
        GetForeignReferences();
    }
    private void CheckInstance()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public static void PauseGame()
    {
        Time.timeScale = 0;
    }
    public static void ResumeGame()
    {
        Time.timeScale = 1;
    }
    private void SetRefreshRateAccordingToDevice()
    { 
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        RefreshRate = Application.targetFrameRate;
        //QualitySettings.vSyncCount = 1;
    }
    private void GetForeignReferences()
    {

    }
}
