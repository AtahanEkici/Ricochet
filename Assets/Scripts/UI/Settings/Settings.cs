using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("Local Components")]
    [SerializeField] private Toggle AutoAimToggle;
    [SerializeField] private Toggle Vsync;
    [SerializeField] private Toggle ShowFPSToggle;
    [SerializeField] private Slider DesiredFPS;
    private void Awake()
    {
        //GetLocalReferences();
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        
    }
    private void GetLocalReferences()
    {
        if(AutoAimToggle == null)
        {
            AutoAimToggle = transform.GetChild(0).GetComponent<Toggle>();
        }
        if (Vsync == null)
        {
            Vsync = transform.GetChild(1).GetComponent<Toggle>();
        }
        if (ShowFPSToggle == null)
        {
            ShowFPSToggle = transform.GetChild(2).GetComponent<Toggle>();
        }
        if (DesiredFPS == null)
        {
            DesiredFPS = transform.GetChild(3).GetComponent<Slider>();
        }
    }
    private void DelegateToggles()
    {
        AutoAimToggle.onValueChanged.AddListener(OnAutoAimChanged);
    }
    private void OnAutoAimChanged(bool value)
    {
        Debug.Log("Auto Aim Changed "+value+"");
        // Do Something //
    }
    private void VsyncChanged()
    {
        Debug.Log("Vsync Changed");
    }
    private void ShowFPSChanged()
    {
        Debug.Log("Show FPS Changed");
    }
}
