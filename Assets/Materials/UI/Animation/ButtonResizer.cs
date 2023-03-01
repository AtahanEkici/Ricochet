using UnityEngine;

public class ButtonResizer : MonoBehaviour
{
    [SerializeField] private float animationSpeed = 200f;
    [SerializeField] private float maxScale = 2.5f;
    [SerializeField] private float minScale = 1.3f;
    private bool expand = true;
    private Transform transformCache;
    private static readonly Vector3 rate = new (0.01f, 0.01f, 0.01f);

    private void Start()
    {
        transformCache = transform;
    }

    private void Update()
    {
        ScaleAnimation(Time.unscaledDeltaTime);
    }

    private void ScaleAnimation(float deltaTime)
    {
        float x = transformCache.localScale.x;

        if (x > maxScale)
        {
            expand = false;
        }
        else if (x <= minScale)
        {
            expand = true;
        }

        if (expand)
        {
            transformCache.localScale += deltaTime * animationSpeed * rate;
        }
        else
        {
            transformCache.localScale -= deltaTime * animationSpeed * rate;
        }
    }
}