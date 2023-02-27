using UnityEngine;
[DefaultExecutionOrder(-1100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private GameManager(){}

    [Header("Screen Operations")]
    [SerializeField] private int RefreshRate = 60;

    [Header("Canvas Container")]
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject LevelPassedPanel;
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject ScorePanel;
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
        if(Instance == null)
        {
            Instance = this;
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
