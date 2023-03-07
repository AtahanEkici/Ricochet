using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[DefaultExecutionOrder(-700)]
public class SliderController : MonoBehaviour
{
    public static SliderController Instance { get; private set; }
    private SliderController() { }

    [Header("Local Components")]
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject FillArea;
    [SerializeField] private TextMeshProUGUI SliderPercentageText;

    [Header("Slider Info")]
    [SerializeField] private static float new_value = 0f;
    [Range (1f,20f)]
    [SerializeField] private float SliderSpeed = 5f;

    [Header("Block Info")]
    [SerializeField] private static int InitialBrickCount = 0;
    private void Awake()
    {
        CheckInstance();
        GetLocalReferences();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void Update()
    {
        LerpSlider();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode SceneLoad)
    {
        SceneLoadOperations(scene);
    }
    private void CheckInstance()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void GetLocalReferences()
    {
        if(slider == null)
        {
            slider = GetComponent<Slider>();
        }

        if(FillArea == null)
        {
            FillArea = transform.GetChild(1).gameObject;
        }

        if(SliderPercentageText == null)
        {
            SliderPercentageText = transform.GetChild(transform.childCount-1).gameObject.GetComponent<TextMeshProUGUI>();
        }
    }
    private void SceneLoadOperations(Scene scene)
    {
        if(scene.name == GameManager.StartMenuName) { Debug.Log("Dont Update Slider on Start Menu"); return; }

        try
        {
            InitialBrickCount = LevelManager.GetBrickCount();
            UpdateSlider();
        }
        catch(Exception e)
        {
            Debug.LogException(e);
        } 
    }
    private void LerpSlider()
    {
        if (slider.value == new_value) { return; }
        slider.value = Mathf.Lerp(slider.value, new_value, Time.unscaledDeltaTime * SliderSpeed);

        if(slider.value >= 99f) { slider.value = 100; }

        SliderPercentageText.text = "%" + (int)slider.value;
    }
    public static void UpdateSlider()
    {        
        try
        {
            new_value = (100 * LevelManager.GetBrickCount()) / InitialBrickCount;
        }
        catch(Exception e)
        {
            new_value = 0f;
            Debug.LogException(e);
        }
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
