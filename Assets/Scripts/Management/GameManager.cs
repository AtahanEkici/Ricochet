using System;
using UnityEngine;
using UnityEngine.SceneManagement;
[DefaultExecutionOrder(-1100)]
public class GameManager : MonoBehaviour
{
    private const string CanvasTag = "Canvas";
    public const string StartMenuName = "StartMenu";
    public static GameManager Instance { get; private set; }
    private GameManager(){}

    [Header("Screen Operations")]
    [SerializeField] private int RefreshRate = 60;

    [Header("Canvas Container")]
    [SerializeField] private GameObject StartMenuPanel;
    [SerializeField] private GameObject MainCanvas;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject LevelPassedPanel;
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject ScorePanel;

    [Header("Other Container")]
    [SerializeField] private GameObject Platform;
    [SerializeField] private GameObject WallGenerator;
    private void Awake()
    {
        CheckInstance();
        SetRefreshRateAccordingToDevice();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnEnable()
    {
        GetForeignReferences();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneLoad)
    {
        UISettings(scene);
        OtherSettings(scene);
    }
    private void CheckInstance()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
    private void OtherSettings(Scene scene)
    {
        if(scene.name == StartMenuName) 
        {
            Platform.SetActive(false);
            WallGenerator.SetActive(false);
        }
    }
    private void UISettings(Scene scene)
    {
        if(scene.name == StartMenuName) { MainCanvas.SetActive(false); return; }
        else { MainCanvas.SetActive(true); }

        if(scene.name == StartMenuName)
        {
            ScorePanel.SetActive(false);
            GameOverPanel.SetActive(false);
            LevelPassedPanel.SetActive(false);
            SettingsPanel.SetActive(false);
        }
        else
        {
            ScorePanel.SetActive(true);
            GameOverPanel.SetActive(false);
            LevelPassedPanel.SetActive(false);
            SettingsPanel.SetActive(false);
        }
    }
    private void GetCanvasRefrences()
    {
        if(MainCanvas == null)
        {
            MainCanvas = GameObject.FindGameObjectWithTag(CanvasTag);
        }
        if(ScorePanel == null)
        {
            ScorePanel = MainCanvas.transform.GetChild(0).gameObject;
        }
        if(GameOverPanel == null)
        {
            GameOverPanel = MainCanvas.transform.GetChild(1).gameObject;
        }
        if (LevelPassedPanel == null)
        {
            LevelPassedPanel = MainCanvas.transform.GetChild(2).gameObject;
        }
        if (SettingsPanel == null)
        {
            SettingsPanel = MainCanvas.transform.GetChild(3).gameObject;
        }
    }
    public void GameOver()
    {
        PauseGame();
        ScorePanel.SetActive(false);
        LevelPassedPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        GameOverPanel.SetActive(true);
    }
    public void LevelPassed()
    {
        PauseGame();
        ScorePanel.SetActive(false);
        LevelPassedPanel.SetActive(true);
        SettingsPanel.SetActive(false);
        GameOverPanel.SetActive(false);
    }
    public void OpenSettings()
    {
        PauseGame();
        SettingsPanel.SetActive(true);
        ScorePanel.SetActive(false);
        LevelPassedPanel.SetActive(false);
        GameOverPanel.SetActive(false);
    }
    public void CloseSettings()
    {
        ResumeGame();
        ScorePanel.SetActive(true);
        LevelPassedPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        GameOverPanel.SetActive(false);
    }
    private void SetRefreshRateAccordingToDevice()
    { 
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        RefreshRate = Application.targetFrameRate;
        //QualitySettings.vSyncCount = 1;
    }
    private void GetForeignReferences()
    {
        GetCanvasRefrences();
        
        if(Platform == null)
        {
            Platform = FindFirstObjectByType<PlatformController>().gameObject;
        }

        if(WallGenerator == null)
        {
            WallGenerator = FindFirstObjectByType<WallGenerate>().gameObject;
        }
    }
}
