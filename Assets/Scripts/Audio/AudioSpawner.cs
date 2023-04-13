using UnityEngine;
using UnityEngine.UI;
public class AudioSpawner : MonoBehaviour
{
    [Header("Is Supposed for Button")]
    [SerializeField] public bool isForButton = true;

    [Header("Audio Clip")]
    [SerializeField] public AudioClip audio_Clip;

    [Header("Button")]
    [SerializeField] private Button button;
    private void Start()
    {
        GetButtonReferences();
        DelegateButton();
    }
    private void GetButtonReferences()
    {
        if (!isForButton) { return; }

        if(button == null)
        {
            button = GetComponent<Button>();
        }
    }
    private void DelegateButton()
    {
        if(isForButton)
        {
            button.onClick.AddListener(AddSong);
        }
        else
        {
            AddSong();
            Destroy(gameObject);
        }
    }

    private void AddSong()
    {
        GameObject temp = new("SongTemp");
        AudioSource As = temp.AddComponent<AudioSource>();

        if(audio_Clip == null)
        {
            As.PlayOneShot(AudioManager.ButtonPush);
            temp.AddComponent<DestroyAfterDelay>().timeForDestruction = AudioManager.ButtonPush.length * 2f;
        }
        else
        {
            As.clip = audio_Clip;
            As.Play();
            temp.AddComponent<DestroyAfterDelay>().timeForDestruction = audio_Clip.length * 2f;
        }
    }
    private void OnDestroy()
    {
        if(isForButton)
        {
            button.onClick.RemoveAllListeners();
        }
    }
}
