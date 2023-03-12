using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class PostProcessing : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance { get { return instance; } }

    [Header("Post Processing")]
    [SerializeField] private PostProcessVolume Post_Processing;
    private void Awake()
    {
        GetReferences();
    }

    private void GetReferences()
    {
        if(Post_Processing == null)
        {
            Post_Processing = GetComponent<PostProcessVolume>();  
        }
    }
}
