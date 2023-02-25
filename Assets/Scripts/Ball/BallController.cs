using UnityEngine;
public class BallController : MonoBehaviour
{
    [Header("Speed Constraints")]
    [SerializeField] private float MaxSpeed = 5f;

    [Header("Local Components")]
    [SerializeField] private Rigidbody2D rb;

    private void Awake()
    {
        GetLocalReferences();
    }
    private void Start()
    {
        GetForeignRefernces();
        rb.AddForce(Vector2.down, ForceMode2D.Impulse);
    }
    private void Update()
    {
        SpeedCheck();
    }

    private void GetLocalReferences()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void GetForeignRefernces()
    {

    }
    private void SpeedCheck()
    {
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, MaxSpeed);
    }
}
