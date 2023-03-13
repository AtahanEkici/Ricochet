using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
public class Settings : MonoBehaviour
{
    public static Settings Instance { get; private set; }
    private Settings() { }
    [Header("Rememberance")]
    private const string AutoAim_PlayerPrefs = "AutoAim";
    private const string AutoPilot_PlayerPrefs = "AutoPilot";
    private const string Vsync_PlayerPrefs = "Vsync";
    private const string MasterAudio = "MasterAudio";
    private const string PostProcessing = "PostProcessing";

    [Header("Remembered Values")]
    [SerializeField] public static bool AutoAim_Status = false;
    [SerializeField] public static bool AutoPilot_Status = false;
    [SerializeField] private float MasterAudio_Status = 1f;

    [Header("Local Components")]
    [SerializeField] private Toggle AutoAimToggle;
    [SerializeField] private Toggle AutoPilotToggle;
    [SerializeField] private Toggle VsyncToggle;
    [SerializeField] private Toggle PostProcessingToggle;
    [SerializeField] private Slider AudioVolumeSlider;
    [SerializeField] private TextMeshProUGUI audioText;
    [SerializeField] private Button CloseButton;
    [SerializeField] private AudioSource audioSource;

    [Header("Foreign Componenets")]
    [SerializeField] private AudioListener Audio_Listener;
    [SerializeField] private PlatformController platform;
    [SerializeField] private PostProcessVolume PostProcess_Manager;
    private void Awake()
    {
        CheckInstance();
        GetLocalReferences();
    }
    private void OnEnable()
    {
        GetForeignReferences();
        DelegateTogglesAndSliders();
    }
    private void Start()
    { 
        GetValues();
    }
    private void CheckInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            try
            {
                Destroy(gameObject);
            }
            catch(Exception e)
            {
                Destroy(this);
                Debug.LogException(e);
            }  
        }
    }
    private void GetLocalReferences()
    {
        if(AutoAimToggle == null)
        {
            AutoAimToggle = transform.GetChild(0).GetComponent<Toggle>();
        }
        if (AutoPilotToggle == null)
        {
            AutoPilotToggle = transform.GetChild(1).GetComponent<Toggle>();
        }
        if (VsyncToggle == null)
        {
            VsyncToggle = transform.GetChild(2).GetComponent<Toggle>();
        }
        if(AudioVolumeSlider == null)
        {
            AudioVolumeSlider = transform.GetChild(3).GetComponent<Slider>();
        }
        if(CloseButton == null)
        {
            CloseButton = transform.GetChild(4).GetComponent<Button>();
        }
        if(audioText == null)
        {
            audioText = AudioVolumeSlider.GetComponentInChildren<TextMeshProUGUI>();
        }
        if(audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        if(PostProcessingToggle == null)
        {
            PostProcessingToggle = transform.GetChild(5).GetComponent<Toggle>();
        }
    }
    private void GetForeignReferences()
    {
        if(Audio_Listener == null)
        {
            Audio_Listener = Camera.main.GetComponent<AudioListener>();
        }
    }
    private void GetValues()
    {
        if (platform == null)
        {
            platform = FindFirstObjectByType<PlatformController>();
        }

        if (PostProcess_Manager == null)
        {
            PostProcess_Manager = GameObject.FindGameObjectWithTag(PostProcessing).GetComponent<PostProcessVolume>();

        }

        switch (PlayerPrefs.GetInt(AutoAim_PlayerPrefs, 0))// Auto Aim Player Prefs //
        {
                case 0:
                AutoAim_Status = false;
                AutoAimToggle.isOn = false;
                platform.AutoAim = AutoAim_Status;
                break;

                case 1:
                AutoAim_Status = true;
                AutoAimToggle.isOn = true;
                platform.AutoAim = AutoAim_Status;
                break;

                default:
                AutoAim_Status = false;
                AutoAimToggle.isOn = false;
                platform.AutoAim = AutoAim_Status;
                break;
        }

        switch (PlayerPrefs.GetInt(AutoPilot_PlayerPrefs, 0)) // Auto Pilot Player Prefs //
        {
            case 0:
                AutoPilot_Status = false;
                AutoPilotToggle.isOn = false;
                platform.AutoPilot = AutoPilot_Status;
                break;

            case 1:
                AutoPilot_Status = true;
                AutoPilotToggle.isOn = true;
                platform.AutoPilot = AutoPilot_Status;
                break;

            default:
                AutoAim_Status = false;
                AutoPilotToggle.isOn = false;
                platform.AutoPilot = AutoPilot_Status;
                break;
        }

        switch (PlayerPrefs.GetInt(Vsync_PlayerPrefs, 0)) // VSync Player Prefs //
        {
            case 0:
                QualitySettings.vSyncCount = 0;
                VsyncToggle.isOn = false;
                break;

            case 1:
                QualitySettings.vSyncCount = 1;
                VsyncToggle.isOn = true;
                break;

            default:
                QualitySettings.vSyncCount = 0;
                VsyncToggle.isOn = false;
                break;
        }

        switch (PlayerPrefs.GetInt(PostProcessing, 1)) // PostProcessing Player Prefs //
        {
            case 0:
                PostProcess_Manager.enabled = false;
                PostProcessingToggle.isOn = false;
                break;

            case 1:
                PostProcess_Manager.enabled = true;
                PostProcessingToggle.isOn = true;
                break;

            default:
                PostProcess_Manager.enabled = true;
                PostProcessingToggle.isOn = true;
                break;
        }

        MasterAudio_Status = PlayerPrefs.GetFloat(MasterAudio, 1f);
        AudioListener.volume = MasterAudio_Status;
        AudioVolumeSlider.value = MasterAudio_Status;
    }
    private void DelegateTogglesAndSliders()
    {
        AutoAimToggle.onValueChanged.AddListener(delegate
        {
            OnAutoAimChanged(AutoAimToggle);
        });

        AutoPilotToggle.onValueChanged.AddListener(delegate
        {
            OnAutoPilotChanged(AutoPilotToggle);
        });

        VsyncToggle.onValueChanged.AddListener(delegate
        {
            VsyncChanged(VsyncToggle);
        });

        PostProcessingToggle.onValueChanged.AddListener(delegate
        {
            PostProcessingToggleChanged(PostProcessingToggle);
        });

        AudioVolumeSlider.onValueChanged.AddListener(delegate
        {
            AudioVolumeChanged(AudioVolumeSlider);
        });

        CloseButton.onClick.AddListener(delegate
        {
            MenuCloseButtonPressed();
        });
    }
    private void OnAutoAimChanged(Toggle toggle)
    {
        if(toggle.isOn)
        {
            AutoAim_Status = true;
            PlayerPrefs.SetInt(AutoAim_PlayerPrefs, 1);
        }
        else
        {
            AutoAim_Status = false;
            PlayerPrefs.SetInt(AutoAim_PlayerPrefs, 0);
        }
        platform.AutoAim = AutoAim_Status;
    }
    private void OnAutoPilotChanged(Toggle toggle)
    {
        if (toggle.isOn)
        {
            AutoPilot_Status = true;
            PlayerPrefs.SetInt(AutoPilot_PlayerPrefs, 1);
        }
        else
        {
            AutoPilot_Status = false;
            PlayerPrefs.SetInt(AutoPilot_PlayerPrefs, 0);
        }
        platform.AutoPilot = AutoPilot_Status;
    }
    private void VsyncChanged(Toggle toggle)
    {
        if (toggle.isOn)
        {
            PlayerPrefs.SetInt(Vsync_PlayerPrefs, 1);
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            PlayerPrefs.SetInt(Vsync_PlayerPrefs, 0);
            QualitySettings.vSyncCount = 0;
        }
    }
    private void PostProcessingToggleChanged(Toggle toggle)
    {
        if (toggle.isOn)
        {
            PlayerPrefs.SetInt(PostProcessing, 1);
            PostProcess_Manager.enabled = true;
        }
        else
        {
            PlayerPrefs.SetInt(PostProcessing, 0);
            PostProcess_Manager.enabled = false;
        }
    }
    private void AudioVolumeChanged(Slider slider)
    {
        MasterAudio_Status = slider.value;
        AudioListener.volume = MasterAudio_Status;

        audioText.text = "Volume: %" + (int)(MasterAudio_Status * 100);

        PlayerPrefs.SetFloat(MasterAudio, MasterAudio_Status);
    }
    private void MenuCloseButtonPressed()
    {
        audioSource.PlayOneShot(AudioManager.FalseClick);
        StartCoroutine(WaitForAudioClip(audioSource)); 
    }

    public static IEnumerator WaitForAudioClip(AudioSource AS)
    {
        yield return new WaitUntil(() => !AS.isPlaying);
        GameManager.Instance.CloseSettings();
        PlayerPrefs.Save();
    }
}
