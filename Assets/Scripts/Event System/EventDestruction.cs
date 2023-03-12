using UnityEngine;
public class EventDestruction : MonoBehaviour
{
    private static EventDestruction instance;
    public static EventDestruction Instance { get { return instance; } }

    private void Awake()
    {
        CheckInstance();
    }
    private void CheckInstance()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
