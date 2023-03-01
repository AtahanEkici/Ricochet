using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMaster : MonoBehaviour
{
    public static UIMaster Instance { get; private set; }
    private UIMaster() {}

    [Header("Don't Destroy object On level Load ?")]
    [SerializeField] private bool DontDestroy = true;

    [Header("Level Number Text")]
    [SerializeField] private TextMeshProUGUI LevelNumberText;

    [Header("Local References")]
    [SerializeField] private GameObject ScorePanel;
    [SerializeField] private GameObject GameOver;
    [SerializeField] private GameObject LevelPassed;
    [SerializeField] private GameObject Settings;
    private void Awake()
    {
        CheckInstance();
        CheckDestruction();
        GetLocalRefrences();
    }
    private void GetLocalRefrences()
    {
        if (ScorePanel == null)
        {
            ScorePanel = transform.GetChild(0).gameObject;
        }
        if (GameOver == null)
        {
            GameOver = transform.GetChild(1).gameObject;
        }
        if (LevelPassed == null)
        {
            LevelPassed = transform.GetChild(2).gameObject;
        }
        if (Settings == null)
        {
            Settings = transform.GetChild(3).gameObject;
        }
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
    private void CheckDestruction()
    {
        if(DontDestroy)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
