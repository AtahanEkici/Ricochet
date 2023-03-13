using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

[DefaultExecutionOrder(-800)]
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; } 
    private ScoreManager() { }

    private const string total_score = "_Score";

    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSource;

    [Header("Score Board")]
    [SerializeField] private static TextMeshProUGUI ScoreUI;
    [SerializeField] private static int Score = 0;

    [Header("Scene Info")]
    [SerializeField] private static string SceneName = "";

    [Header("Open Menu Button")]
    [SerializeField] private Button OpenMenuButton;
    private void Awake()
    {
        CheckInstance();
        GetLocalReferences();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnEnable()
    {
        if(OpenMenuButton == null)
        {
            OpenMenuButton = transform.GetChild(2).GetComponent<Button>();
            OpenMenuButton.onClick.AddListener(OnOpenMenuButtonPressed);
        }
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
    private void OnOpenMenuButtonPressed()
    {
        audioSource.PlayOneShot(AudioManager.FalseClick);
        StartCoroutine(WaitForAudioClip(audioSource));
    }
    public static IEnumerator WaitForAudioClip(AudioSource AS)
    {
        yield return new WaitUntil(() => !AS.isPlaying);
        GameManager.Instance.OpenSettings();
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
        if (ScoreUI == null)
        {
            ScoreUI = transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        }

        if(audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            
        }
    }
    private void GetForeignReferences()
    {
        if(audioSource.clip == null)
        {
            audioSource.clip = AudioManager.ButtonPush;
        }  
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
        PlayerPrefs.Save();
    }
}
