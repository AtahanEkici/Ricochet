using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class PostProcessing : MonoBehaviour
{
    private static PostProcessing instance;
    public static PostProcessing Instance { get { return instance; } }

    [Header("Post Processing")]
    [SerializeField] private PostProcessVolume Post_Processing;
    private void Awake()
    {
        CheckInstance();
        GetReferences();
    }
    private void CheckInstance()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void GetReferences()
    {
        if(Post_Processing == null)
        {
            Post_Processing = GetComponent<PostProcessVolume>();  
        }
    }
}
