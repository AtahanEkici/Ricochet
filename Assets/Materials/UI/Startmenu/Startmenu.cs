using UnityEngine;
using UnityEngine.UI;
public class Startmenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject ButtonsPanel;
    [SerializeField] private GameObject LevelsPanel;

    [Header("Level Buttons")]
    [SerializeField] private Button[] LevelButtons;
    private void Awake()
    {
        GetReferences();
    }
    private void OnEnable()
    {
        CloseLevelPanel();
    }
    private void Start()
    {
        DelegateLevelButtons();
    }
    public void OpenLevelsPanel()
    {
        ButtonsPanel.SetActive(false);
        LevelsPanel.SetActive(true);
    }
    public void CloseLevelPanel()
    {
        ButtonsPanel.SetActive(true);
        LevelsPanel.SetActive(false);
    }
    private void GetReferences()
    {
        if(ButtonsPanel == null)
        {
            ButtonsPanel = transform.GetChild(0).gameObject;
        }

        if(LevelsPanel == null)
        {
            LevelsPanel = transform.GetChild(1).gameObject;
        }
    }
    private void DelegateLevelButtons()
    {
        LevelButtons = LevelsPanel.GetComponentsInChildren<Button>();

        int MaxReachedlevel = LevelManager.GetMaxReachedLevel();

        Debug.Log("Max Reached Level: " +MaxReachedlevel);

        for(int i=0;i<LevelButtons.Length;i++)
        {
            Button button = LevelButtons[i];

            if (MaxReachedlevel < int.Parse(button.name))
            {
                button.interactable = false;
            }
            else
            {
                button.onClick.AddListener(delegate { Debug.Log("Pressed: " + button.name + ""); LevelManager.LoadLevel(button.name); });
                button.gameObject.AddComponent<CradleBehaviour>();
                button.gameObject.AddComponent<AudioSpawner>();
            }
        }
    }
}

