using UnityEngine;
using UnityEngine.UI;
public class ToGenerativeLevel : MonoBehaviour
{
    [Header("Generative Level Button")]
    [SerializeField] private Button GenerativeLevelButton;

    private void Awake()
    {
        GetLocalReferences();
    }
    private void OnEnable()
    {
        DelegateButton();
    }
    private void GetLocalReferences()
    {
        if(GenerativeLevelButton == null)
        {
            GenerativeLevelButton = GetComponent<Button>();
        }
    }
    private void DelegateButton()
    {
        GenerativeLevelButton.onClick.AddListener(GenerativeLevel);
    }
    private void GenerativeLevel()
    {
        LevelManager.LoadLevel(GameManager.GenerativeLevel);
    }
}
