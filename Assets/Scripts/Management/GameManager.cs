using System;
using UnityEngine;
using UnityEngine.SceneManagement;
[DefaultExecutionOrder(-1100)]
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public const string StartMenuName = "StartMenu";
    public static GameManager Instance { get { return instance; } }
    private GameManager(){}

    [Header("Screen Operations")]
    [SerializeField] private int RefreshRate = 60;

    [Header("Canvas Container")]
    [SerializeField] private GameObject MainCanvas;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject LevelPassedPanel;
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject ScorePanel;

    [Header("Other Container")]
    [SerializeField] private GameObject WallGenerator;
    private void Awake()
    {
        CheckInstance();
        SetRefreshRateAccordingToDevice();
        SceneManager.sceneLoaded += OnSceneLoaded;
        GetCanvasReferences();
    }
    private void OnEnable()
    {
        GetForeignReferences();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneLoad)
    {
        //GetCanvasReferences();
        UISettings(scene);
        OtherSettings(scene);
    }
    private void CheckInstance()
    {
        if(instance == null)
        {
            instance = this;
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
            WallGenerator.SetActive(false);
        }
        else
        {
            WallGenerator.SetActive(true);
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
    private void GetCanvasReferences()
    {
        try
        {
            MainCanvas = FindFirstObjectByType<UIMaster>().gameObject;
            ScorePanel = MainCanvas.transform.GetChild(0).gameObject;
            GameOverPanel = MainCanvas.transform.GetChild(1).gameObject;
            LevelPassedPanel = MainCanvas.transform.GetChild(2).gameObject;
            SettingsPanel = MainCanvas.transform.GetChild(3).gameObject;
        }
        catch(Exception e)
        {
            Debug.LogException(e);
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

        if(WallGenerator == null)
        {
            WallGenerator = FindFirstObjectByType<WallGenerate>().gameObject;
        }
    }
}
