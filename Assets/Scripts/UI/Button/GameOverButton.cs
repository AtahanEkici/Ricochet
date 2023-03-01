using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class GameOverButton : MonoBehaviour
{
    private static GameOverButton instance;
    public static GameOverButton Instance { get { return instance; } }

    [Header("Game Over Button")]
    [SerializeField] private Button button;

    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSource;
    private void Awake()
    {
        CheckInstance();
    }
    private void OnEnable()
    {
        GetReferences();
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
    private void GetReferences()
    {
        if(button == null)
        {
            button = GetComponent<Button>();
        }

        if(audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
    private void DelegateButton()
    {
        button.onClick.AddListener(ClickedGameOverButton);
    }
    private void ClickedGameOverButton()
    {
        audioSource.PlayOneShot(AudioManager.ButtonPush);
        StartCoroutine(WaitAndExecute());
    }
    private IEnumerator WaitAndExecute()
    {
        yield return new WaitWhile(() => !audioSource.isPlaying);
        LevelManager.RestartLevel();
    }
}
