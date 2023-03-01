using System;
using UnityEngine;
public class BallController : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private const string WallTag = "Wall";
    [SerializeField] private const string BallTag = "Ball";
    [SerializeField] private const string PlatformTag = "Platform";
    [SerializeField] private const string BrickTag = "Brick";
    [SerializeField] private const string BottomWallName = "Bottom";

    [Header("Speed Constraints")]
    [SerializeField] private float MaxSpeed = 10f;
    [SerializeField] private float MinSpeed = 5f;

    [Header("Local Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private AudioSource Audio_Source;

    [Header("Damage Attribute")]
    [SerializeField] private int DamageNumber = 1;

    [Header("Before Collided")]
    [SerializeField] private GameObject BeforeCollided = null;
    [SerializeField] private int LoopThreshold = 2;
    [SerializeField] private int LoopCounter = 0;

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
    }
    private void LateUpdate()
    {
        SpeedCheck();
    }
    private void CheckloopingBehaviour(GameObject collidedObject) 
    {
        try
        {
            if(BeforeCollided == null)
            {
                BeforeCollided = collidedObject;
                return;
            }
            else if (BeforeCollided != collidedObject)
            {
                if ((BeforeCollided.name.Contains("Left") && collidedObject.name.Contains("Right")) || (BeforeCollided.name.Contains("Right") && collidedObject.name.Contains("Left")))
                {
                    if (LoopCounter++ >= LoopThreshold)
                    {
                        SendBallBackToPlatform();
                        LoopCounter = 0;
                    }
                }
                else
                {
                    LoopCounter = 0;
                }
                BeforeCollided = collidedObject;
            }
        }
        catch(Exception e)
        {
            Debug.Log(e.StackTrace + " -BallController- " + e.Message);
        }
    }
    public float GetMinSpeed()
    {
        return MinSpeed;
    }
    public float GetMaxSpeed()
    {
        return MaxSpeed;
    }
    private void SendBallBackToPlatform()
    {
        GameObject Platform = FindFirstObjectByType<PlatformController>().gameObject;
        Vector2 PlatformVector = Platform.transform.position;
        Vector2 ToPlatform = PlatformVector - (Vector2)transform.position;
        Debug.DrawRay(transform.position, ToPlatform, Color.cyan,2f);
        Debug.Log("Sending Ball To Platform");
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.AddForce(ToPlatform,ForceMode2D.Impulse);

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

        CheckloopingBehaviour(go);

        if (go.CompareTag(WallTag) && go.name != BottomWallName)
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

            else if(bc.GetHealth() > 1)
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
