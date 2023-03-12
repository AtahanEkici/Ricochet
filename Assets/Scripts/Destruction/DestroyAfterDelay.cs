using UnityEngine;
public class DestroyAfterDelay : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] public float timeForDestruction = 1f;

    private void Update()
    {
        CheckTimer();
    }

    private void CheckTimer()
    {
        timeForDestruction -= Time.unscaledDeltaTime;

        if(timeForDestruction <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
