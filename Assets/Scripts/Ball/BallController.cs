using UnityEngine;
public class BallController : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private const string WallTag = "Wall";
    [SerializeField] private const string BallTag = "Ball";
    [SerializeField] private const string PlatformTag = "Platform";
    [SerializeField] private const string BrickTag = "Brick";
    [SerializeField] private const string BottomWallName = "BottomWall";

    [Header("Speed Constraints")]
    [SerializeField] private float MaxSpeed = 10f;
    [SerializeField] private float MinSpeed = 5f;

    [Header("Local Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private AudioSource Audio_Source;

    [Header("Damage Attribute")]
    [SerializeField] private int DamageNumber = 1;

    private void Awake()
    {
        GetLocalReferences();
    }
    private void OnEnable()
    {
        LevelManager.AddToBalls(gameObject);
    }
    private void Start()
    {
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
        Audio_Source = GetComponent<AudioSource>();
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
        Debug.Log("Ball Velocity: "+rb.velocity.ToString());
    }
    private static Vector2 ClampMagnitude(Vector2 v, float max, float min)
    {
        float sm = v.sqrMagnitude;
        if (sm > (float)max * (float)max) return v.normalized * max;
        else if (sm < (float)min * (float)min) return v.normalized * min;
        return v;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject go = collision.gameObject;

        if(go.CompareTag(WallTag) && go.name != BottomWallName)
        {
            Audio_Source.PlayOneShot(AudioManager.WallHit);
        }
        else if(go.CompareTag(PlatformTag))
        {
            Audio_Source.PlayOneShot(AudioManager.PlatformHit);
        }
        else if (go.CompareTag(BrickTag))
        {
            BrickController bc = go.GetComponent<BrickController>();
            
            if (bc == null) { return; }

            else if(bc.GetHealth() > 0)
            {

                Audio_Source.PlayOneShot(AudioManager.Dent);
            }

            else
            {
                Audio_Source.PlayOneShot(AudioManager.BrickDestroy); 
            }
        }
    }
}
