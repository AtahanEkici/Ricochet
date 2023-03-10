using UnityEngine;
using UnityEngine.UI;
public class CradleBehaviour : MonoBehaviour
{
    [Header("Direction")]
    [SerializeField] private bool isLeft = false;

    [Header("Direction Probability")]
    [Range(0f, 1f)]
    [SerializeField] private float Probability = 0.5f;

    [Header("Local Components")]
    [SerializeField] private RectTransform trans;

    [Header("Rotation Constraints")]
    [SerializeField] private float RotationSpeed = 50f;
    [SerializeField] private float MaxAngle = 0.1f;

    [Header("Angles")]
    [SerializeField] private float RightAngle = 0f;
    [SerializeField] private float LeftAngle = 0f;

    [Header("Destinations")]
    [SerializeField] private Quaternion Left = Quaternion.Euler(0,0,15);
    [SerializeField] private Quaternion Right = Quaternion.Euler(0, 0,-15);

    [Header("Does Button have a sound")]
    [SerializeField] private bool HaveSound = false;
    [SerializeField] private AudioSource Audio_Source;
    [SerializeField] private Button thisButton;
    private void Awake()
    {
        GetReferences();
    }
    private void Update()
    {
        Cradle();
    }
    private void Cradle()
    {
       if(!isLeft)
       {
            trans.rotation = Quaternion.RotateTowards(trans.rotation, Right, Time.smoothDeltaTime * RotationSpeed);
            RightAngle = Quaternion.Angle(trans.rotation, Right);

            if (RightAngle <= MaxAngle)
            {
                isLeft = true;
            }
        }
       else
       {
            trans.rotation = Quaternion.RotateTowards(trans.rotation, Left, Time.smoothDeltaTime * RotationSpeed);
            LeftAngle = Quaternion.Angle(trans.rotation, Left);

            if (LeftAngle <= MaxAngle)
            {
                isLeft = false;
            }
        }
    }
    private void GetReferences()
    {
        trans = GetComponent<RectTransform>();
        Right = Quaternion.Inverse(Left);
        isLeft = Random.value > Probability;

        if(HaveSound && Audio_Source == null)
        {
            Audio_Source = GetComponent<AudioSource>();
        }

        if(HaveSound && thisButton == null)
        {
            thisButton = GetComponent<Button>();
            DelegateButton();
        }
    }
    private void ButtonClick()
    {
        if(Audio_Source.clip == null)
        {
            Audio_Source.PlayOneShot(AudioManager.ButtonPush);
        }
        else
        {
            Audio_Source.Play();
        }  
    }
    private void DelegateButton()
    {
        if(thisButton != null)
        {
            thisButton.onClick.AddListener(ButtonClick);
        }
    }
}