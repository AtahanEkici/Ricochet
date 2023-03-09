using UnityEngine;

public class ButtonResizer : MonoBehaviour
{
    private bool Expend = true;

    [SerializeField] private float Animation_Speed = 200f;

    [SerializeField] private Vector3 scale;
    [SerializeField] private Vector3 rate = new Vector3(0.01f, 0.01f, 0.01f);

    [SerializeField] private float MAX_Scale = 2.5f;
    [SerializeField] private float MIN_Scale = 1.0f;

    private void Start()
    {
        scale = gameObject.transform.localScale;
    }
    private void Update()
    {
        ScaleAnimation(Time.unscaledDeltaTime);
    }
    private void ScaleAnimation(float a)
    {
        Vector3 LocalScale = gameObject.transform.localScale;

        if (LocalScale.x > MAX_Scale)
        {
            gameObject.transform.localScale = new(MAX_Scale, MAX_Scale, MAX_Scale);
            Expend = false;
        }
        else if (LocalScale.x < MIN_Scale)
        {
            gameObject.transform.localScale = new(MIN_Scale, MIN_Scale, MIN_Scale);
            Expend = true;
        }

        if (Expend == true)
        {
            gameObject.transform.localScale += a * Animation_Speed * rate;
        }
        else if (Expend == false)
        {
            gameObject.transform.localScale -= a * Animation_Speed * rate;
        }
        else
        {
            gameObject.transform.localScale = scale;
        }
    }
}