using UnityEngine;
using UnityEngine.UI;
public class AudioSpawner : MonoBehaviour
{
    [Header("Audio Clip")]
    [SerializeField] private AudioClip audio_Clip;

    [Header("Button")]
    [SerializeField] private Button button;
    private void Awake()
    {
        GetButtonReferences();
    }
    private void OnEnable()
    {
        DelegateButton();
    }
    private void GetButtonReferences()
    {
        if(button == null)
        {
            button = GetComponent<Button>();
        }
    }
    private void DelegateButton()
    {
        button.onClick.AddListener(AddSong);
    }

    private void AddSong()
    {
        GameObject temp = new("SongTemp");
        AudioSource As = temp.AddComponent<AudioSource>();

        if(audio_Clip == null)
        {
            As.PlayOneShot(AudioManager.ButtonPush);
            temp.AddComponent<DestroyAfterDelay>().timeForDestruction = AudioManager.ButtonPush.length;
        }
        else
        {
            As.clip = audio_Clip;
            As.Play();
            temp.AddComponent<DestroyAfterDelay>().timeForDestruction = audio_Clip.length;
        }

        
    }
}
