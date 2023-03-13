using UnityEngine;
using UnityEngine.UI;
public class PrivacyPolicy : MonoBehaviour
{
    [Header("Privacy Policy Button")]
    [SerializeField] private Button PrivacyPolicyButton;
    private void Awake()
    {
        GetLocalComponenets();
    }
    private void OnEnable()
    {
        DelegateButton();
    }
    private void GetLocalComponenets()
    {
        if(PrivacyPolicyButton == null)
        {
            PrivacyPolicyButton = GetComponent<Button>();
        }
    }
    private void DelegateButton()
    {
        PrivacyPolicyButton.onClick.AddListener(OpenPolicyURL);
    }
    private void OpenPolicyURL()
    {
        Application.OpenURL("https://sites.google.com/view/ricochet2d-privacy-policy/ana-sayfa"); 
    }
}
