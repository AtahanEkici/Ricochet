using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
[DefaultExecutionOrder(-800)]
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; } 
    private ScoreManager() { }

    private const string total_score = "_Score";

    [Header("Score Board")]
    [SerializeField] private static TextMeshProUGUI ScoreUI;
    [SerializeField] private const string ScoreText = "SCORE: ";
    [SerializeField] private static int Score = 0;

    [Header("Scene Info")]
    [SerializeField] private static string SceneName = "";
    private void Awake()
    {
        CheckInstance();
        GetLocalReferences();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void Start()
    {
        GetForeignReferences();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        UpdateSceneName();
        CheckScore();
        UpdateScoreUI();
    }
    private void CheckInstance()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.parent);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void GetLocalReferences()
    {
        ScoreUI = transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    }
    private void GetForeignReferences()
    {
        
    }
    private static void CheckScore()
    {
        string total = (SceneName + total_score).Trim();

        if (PlayerPrefs.HasKey(total))
        {
            if (Score > PlayerPrefs.GetInt(total))
            {
                PlayerPrefs.SetInt(total,Score);
            }
            else
            {
                Score = PlayerPrefs.GetInt(total);
            }
        }
        else
        {
            PlayerPrefs.SetInt(total, 0);
        }
    }
    private void UpdateSceneName()
    {
        SceneName = SceneManager.GetActiveScene().name;
    }
    private static void UpdateScoreUI()
    {
        if (ScoreUI == null) { return; }

        ScoreUI.text = "Score: " + Score.ToString();
    }
    public static void UpdateScore(int score_point)
    {
        Score += score_point;
        CheckScore();
        UpdateScoreUI();
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}