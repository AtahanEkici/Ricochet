using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class LevelPassed : MonoBehaviour
{
    private static LevelPassed instance;
    public static LevelPassed Instance { get { return instance; } }
    [Header("Level Passed Button")]
    [SerializeField] private Button LevelPassedButton;

    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSource;
    private void Awake()
    {
        CheckInstance();
        GetLocalReferences();
    }
    private void OnEnable()
    {
        DelegateButton();
    }
    private void CheckInstance()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void GetLocalReferences()
    {
        if(LevelPassedButton == null)
        {
            LevelPassedButton = GetComponent<Button>();
        }
        if(audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
    private void DelegateButton()
    {
        LevelPassedButton.onClick.AddListener(LevelPassedButtonClicked);
    }
    private void LevelPassedButtonClicked()
    {
        ClickedGameOverButton();
    }
    private void ClickedGameOverButton()
    {
        audioSource.PlayOneShot(AudioManager.ButtonPush);
        StartCoroutine(WaitAndExecute());
    }
    private IEnumerator WaitAndExecute()
    {
        yield return new WaitWhile(() => !audioSource.isPlaying);
        LevelManager.LevelPassed();
    }
}
