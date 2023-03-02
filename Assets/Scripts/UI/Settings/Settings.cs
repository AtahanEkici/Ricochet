using System;
using UnityEngine;
using UnityEngine.UI;
public class Settings : MonoBehaviour
{
    public static Settings Instance { get; private set; }
    private Settings() { }
    [Header("Rememeberance")]
    private const string AutoAim_PlayerPrefs = "AutoAim";
    private const string AutoPilot_PlayerPrefs = "AutoPilot";
    private const string DesiredFPS_PlayerPrefs = "DesiredFPS";
    private const string Vsync_PlayerPrefs = "Vsync";
    private const string MasterAudio = "MasterAudio";

    [Header("Remembered Values")]
    [SerializeField] private bool AutoAim_Status = false;
    [SerializeField] private bool AutoPilot_Status = false;
    [SerializeField] private int DesiredFPS_Status = 30;
    [SerializeField] private bool Vsync_Status = false;
    [SerializeField] private float MasterAudio_Status = 1f;

    [Header("Local Components")]
    [SerializeField] private Toggle AutoAimToggle;
    [SerializeField] private Toggle AutoPilot;
    [SerializeField] private Toggle Vsync;
    [SerializeField] private Toggle ShowFPSToggle;
    [SerializeField] private Slider DesiredFPS;
    [SerializeField] private Slider AudioVolume;
    [SerializeField] private Button CloseButton;
    private void Awake()
    {
        CheckInstance();
        GetLocalReferences();
    }
    private void OnEnable()
    {
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
                Debug.Log(e.StackTrace + "" + e.Message);
            }  
        }
    }
    private void GetLocalReferences()
    {
        if(AutoAimToggle == null)
        {
            AutoAimToggle = transform.GetChild(0).GetComponent<Toggle>();
        }
        if (AutoPilot == null)
        {
            AutoPilot = transform.GetChild(1).GetComponent<Toggle>();
        }
        if (Vsync == null)
        {
            Vsync = transform.GetChild(2).GetComponent<Toggle>();
        }
        if (DesiredFPS == null)
        {
            DesiredFPS = transform.GetChild(3).GetComponent<Slider>();
        }
        if(AudioVolume == null)
        {
            AudioVolume = transform.GetChild(4).GetComponent<Slider>();
        }
        if(CloseButton == null)
        {
            CloseButton = transform.GetChild(5).GetComponent<Button>();
        }
    }
    private void GetValues()
    {
        switch (PlayerPrefs.GetInt(AutoAim_PlayerPrefs, 0))
        {
                case 0:
                    AutoAim_Status = false;
                    break;
                case 1:
                    AutoAim_Status = true;
                    break;
                default:
                    AutoAim_Status = false;
                    break;
        }

        switch (PlayerPrefs.GetInt(AutoPilot_PlayerPrefs, 0))
        {
            case 0:
                AutoPilot_Status = false;
                break;
            case 1:
                AutoPilot_Status = true;
                break;
            default:
                AutoAim_Status = false;
                break;
        }

        switch (PlayerPrefs.GetInt(Vsync_PlayerPrefs, 0))
        {
            case 0:
                Vsync_Status = false;
                break;
            case 1:
                Vsync_Status = true;
                break;
            default:
                Vsync_Status = false;
                break;
        }
        DesiredFPS_Status = PlayerPrefs.GetInt(DesiredFPS_PlayerPrefs, 30);
        MasterAudio_Status = PlayerPrefs.GetInt(MasterAudio, 0);
    }
    private void DelegateTogglesAndSliders()
    {
        AutoAimToggle.onValueChanged.AddListener(delegate
        {
            OnAutoAimChanged(AutoAimToggle);
        });

        AutoPilot.onValueChanged.AddListener(delegate
        {
            OnAutoPilotChanged(AutoPilot);
        });

        Vsync.onValueChanged.AddListener(delegate
        {
            VsyncChanged(Vsync);
        });

        DesiredFPS.onValueChanged.AddListener(delegate
        {
            DesiredFPSSliderChanged(DesiredFPS);
        });

        AudioVolume.onValueChanged.AddListener(delegate
        {
            AudioVolumeChanged(AudioVolume);
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
        Debug.Log("Auto Aim Changed " + AutoAim_Status + "");
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
        Debug.Log("Auto Pilot Changed " + AutoPilot_Status + "");
    }
    private void VsyncChanged(Toggle toggle)
    {
        if (toggle.isOn)
        {
            Vsync_Status = true;
            PlayerPrefs.SetInt(Vsync_PlayerPrefs, 1);
        }
        else
        {
            Vsync_Status = false;
            PlayerPrefs.SetInt(Vsync_PlayerPrefs, 0);
        }
        Debug.Log("Vsync Changed " + Vsync_Status + "");
    }
    private void DesiredFPSSliderChanged(Slider slider)
    {
        Debug.Log("Show DesiredFPS Slider Changed: "+ slider.value + "");

        
    }
    private void AudioVolumeChanged(Slider slider)
    {
        Debug.Log("Audio Volume Slider Changed  " + slider.value + "");
    }
    private void MenuCloseButtonPressed()
    {
        Debug.Log("Menu Close Pressed");

        PlayerPrefs.Save();
    }
}
