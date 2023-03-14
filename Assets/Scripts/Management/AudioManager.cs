using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[DefaultExecutionOrder(-900)]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    private AudioManager(){ }

    [Header("Audio Clips")]
    [SerializeField] private List<AudioClip> AudioClips = new();

    [Header("Audio Sources")]
    [SerializeField] private static readonly string AssetsFileDirectory = "Audio";

    [Header("Audio Names")]
    [SerializeField] private const string ExplosionTrack = "Explosion";
    [SerializeField] private const string PlatformHitTrack = "PlatformHit";
    [SerializeField] private const string WallHitTrack = "WallHit";
    [SerializeField] private const string ButtonPushTrack = "ButtonPush";
    [SerializeField] private const string PowerUpGainedTrack = "PowerUpGained";
    [SerializeField] private const string FalseClickTrack = "FalseClick";
    [SerializeField] private const string DentTrack = "Dent";
    [SerializeField] private const string BrickDestroyTrack = "BrickDestroy";

    [Header("Specified Clips")]
    [SerializeField] public static AudioClip Explosion;
    [SerializeField] public static AudioClip WallHit;
    [SerializeField] public static AudioClip PlatformHit;
    [SerializeField] public static AudioClip ButtonPush;
    [SerializeField] public static AudioClip PowerUpGain;
    [SerializeField] public static AudioClip FalseClick;
    [SerializeField] public static AudioClip Dent;
    [SerializeField] public static AudioClip BrickDestroy;
    private void Awake()
    {
        CheckInstance();
        GetAudioAssets();
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
    private void SetAudiosToMax()
    {
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        for (int i=0;i< audioSources.Length;i++)
        {
            audioSources[i].volume = 1.0f;
        }
    }
    private void GetAudioAssets()
    {
        AudioClips = Resources.LoadAll<AudioClip>(AssetsFileDirectory).ToList();

        for(int i=0;i<AudioClips.Count;i++)
        {
            GetStaticReferences(AudioClips[i].name,i);
        }
    }
    private void GetStaticReferences(string FileName, int index)
    {
        switch (FileName)
        {
            case ExplosionTrack:
                Explosion = AudioClips[index];
                break;

            case PlatformHitTrack:
                PlatformHit = AudioClips[index];
                break;

            case WallHitTrack:
                WallHit = AudioClips[index];
                break;

            case ButtonPushTrack:
                ButtonPush = AudioClips[index];
                break;

            case PowerUpGainedTrack:
                PowerUpGain = AudioClips[index];
                break;

            case FalseClickTrack:
                FalseClick = AudioClips[index];
                break;

            case BrickDestroyTrack:
                BrickDestroy = AudioClips[index];
                break;

            case DentTrack:
                Dent = AudioClips[index];
                break;

            default:
                Debug.Log("Error on loading sound " + FileName + " not found on index " + index + "");
                break;

        }
    }
}
