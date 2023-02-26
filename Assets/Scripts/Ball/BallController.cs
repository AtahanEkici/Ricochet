using UnityEngine;
public class BallController : MonoBehaviour
{
    [Header("Last Collided Object")]
    [SerializeField] private GameObject LastCollided = null;

    [Header("Speed Constraints")]
    [SerializeField] private float MaxSpeed = 5f;
    [SerializeField] private float MinSpeed = 5f;

    [Header("Local Components")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Damage Attribute")]
    [SerializeField] private int DamageNumber = 1;

    private void Awake()
    {
        GetLocalReferences();
    }
    private void Start()
    {
        LevelManager.Instance.AddToBalls(gameObject);
        GetForeignReferences();
        rb.AddForce(Vector2.down, ForceMode2D.Impulse);
    }
    private void LateUpdate()
    {
        SpeedCheck();
    }
    private void GetLocalReferences()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void GetForeignReferences()
    {

    }
    public int GetDamageNumber()
    {
        return DamageNumber;
    }
    private void SpeedCheck()
    {
        rb.velocity = ClampMagnitude(rb.velocity, MaxSpeed, MinSpeed);
    }
    private static Vector3 ClampMagnitude(Vector2 v, float max, float min)
    {
        float sm = v.sqrMagnitude;
        if (sm > (float)max * (float)max) return v.normalized * max;
        else if (sm < (float)min * (float)min) return v.normalized * min;
        return v;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(LastCollided != collision.gameObject)
        {
            LastCollided = collision.gameObject;
        }
    }
}
