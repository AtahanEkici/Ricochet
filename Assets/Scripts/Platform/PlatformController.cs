using UnityEngine;
public class PlatformController : MonoBehaviour
{
    public static PlatformController Instance { get; private set; }
    private PlatformController() { }

    private const string BallTag = "Ball";

    [Header("Auto Aim")]
    [SerializeField] public bool AutoAim = false;

    [Header("Auto Pilot")]
    [SerializeField] public bool AutoPilot = false;

    [Header("Draw Debugging Rays")]
    [SerializeField] private bool isDebugging = false;
    [SerializeField] private float RayHeight = 10f;

    [Header("Controller Position")]
    [SerializeField] private Vector2 MousePosition = Vector2.zero;

    [Header("Movement Direction")]
    [SerializeField] private bool GoingLeft = false;
    [SerializeField] private bool GoingRight = false;

    [Header("Movement")]
    [SerializeField] private Vector3 MovementVector = Vector3.zero;
    [SerializeField] private float MaxSpeed = 5f;
    [SerializeField] private float Platform_Move_Speed = 5f;
    [SerializeField] private float StopOffset = 0f;

    [Header("Trajectory Vectors")]
    [SerializeField] Vector3 normal = Vector3.zero;
    [SerializeField] Vector3 leftish = Vector3.zero;
    [SerializeField] Vector3 rightish = Vector3.zero;

    [Header("Local Components")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Foreign References")]
    [SerializeField] private Camera cam;
    [SerializeField] private WallGenerate WallGenerator;

    [Header("SmoothFixedDeltaTime")]
    [SerializeField] private float smoothFixedDeltaTime = 0f;
    [SerializeField] private int numFrames = 10;
    [SerializeField] private float[] deltaTimeArray;
    [SerializeField] private int index;

    [Header("Ball ýnformation")]
    [SerializeField] Transform BallTransform;
    private void Awake()
    {
        CheckInstance();
        GetLocalReferences();
    }
    private void Start()
    {
        GetForeignReferences();
        CheckRemebrance();
    }
    private void FixedUpdate()
    {
        CalculateSmoothDeltaTime();
        MovePlatformToMouseCoordinates();
        AutoPilotPlatform();
    }
    private void Update()
    {
        SpeedCheck();
        Debugging();
    }
    private void LateUpdate()
    {
        GetMousePosition();
    }
    private void CheckInstance()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void AutoPilotPlatform()
    {
        if (!AutoPilot) { return; }

        BallTransform = LevelManager.ReturnABall();

        if (BallTransform == null) { return; }

        Vector2 BallPos = BallTransform.position;
        Vector2 pos = rb.position;

        MovementVector = Vector2.MoveTowards(new(pos.x, 0f), new(BallPos.x, 0f), Platform_Move_Speed * smoothFixedDeltaTime);

        float Posx = MovementVector.x;

        if (Posx < -StopOffset || Posx > StopOffset) { return; }
        rb.MovePosition(MovementVector);
    }
    private void CalculateSmoothDeltaTime()
    {
        float sum = 0f;
        deltaTimeArray[index] = Time.fixedDeltaTime;
        index = (index + 1) % numFrames;
        for (int i = 0; i < numFrames; i++)
        {
            sum += deltaTimeArray[i];
        }
        smoothFixedDeltaTime = sum / numFrames;
    }
    private void CheckRemebrance()
    {
        Settings.AutoAim_Status = AutoAim;
        Settings.AutoPilot_Status = AutoPilot;
    }
    private void Debugging()
    {
        normal = transform.right;
        leftish = transform.up + transform.right;
        rightish = transform.right - transform.up;

        if (!isDebugging) { return; }
        Debug.DrawRay(transform.position, normal * RayHeight, Color.blue);
        Debug.DrawRay(transform.position, leftish * RayHeight, Color.red);
        Debug.DrawRay(transform.position, rightish * RayHeight, Color.magenta);
    }
    private void SpeedCheck()
    {
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, MaxSpeed) ;
    }
    private void GetLocalReferences()
    {
        deltaTimeArray = new float[numFrames];
        index = 0;

        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }
    private void GetForeignReferences()
    {
        if(cam == null)
        {
            cam = Camera.main;
        }
        
        if(WallGenerator == null)
        {
            WallGenerator = WallGenerate.Instance;
        }

        float haltOfThePlatform = transform.localScale.y;
        float halfOfTheWall = WallGenerator.transform.GetChild(0).localScale.x / 2;
        float RightWallPos = WallGenerator.right.x;

        StopOffset = RightWallPos - (halfOfTheWall + haltOfThePlatform);
    }
    private void MovePlatformToMouseCoordinates()
    {
        if (!Input.GetMouseButton(0)) { GoingLeft = false; GoingRight = false; return; }

        if (AutoPilot) { return; }

        Vector2 pos = rb.position;

        if (pos != MousePosition)
        {
            MovementVector = Vector2.MoveTowards(new(pos.x, 0f), new(MousePosition.x, 0f), Platform_Move_Speed * smoothFixedDeltaTime);

            float Posx = MovementVector.x;
            float MouseX = MousePosition.x;

            if (Posx < -StopOffset || Posx > StopOffset) { return; }
            rb.MovePosition(MovementVector);

            if(MouseX > Posx) // Going Left //
            {
                GoingRight = true;
                GoingLeft = false;
            }
            else // Going Right //
            {
                GoingRight = false;
                GoingLeft = true;
            }
        }
    }
    private void GetMousePosition()
    {
        MousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
    }
    private void SendBallToClosestBrick(GameObject go)
    {
        Rigidbody2D rb2d = go.GetComponent<Rigidbody2D>();

        rb2d.velocity = Vector2.zero;
        rb2d.angularVelocity = 0f;
        Vector2 owntransform = go.transform.position;
        Vector2 ClosestBrick = LevelManager.Instance.GetClosestBrickCoordinates(owntransform);
        Vector2 ToClosest = ClosestBrick - owntransform;
        rb2d.AddForce(ToClosest,ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject CollidedObject = collision.gameObject;

        if(AutoAim)
        {
            if(CollidedObject.CompareTag(BallTag))
            {
                SendBallToClosestBrick(CollidedObject);
            }
        }
        else
        {
            if(CollidedObject.CompareTag(BallTag))
            {
                Rigidbody2D ballsRigidbody = CollidedObject.GetComponent<Rigidbody2D>();
                float BallsMass = ballsRigidbody.mass;

                if (GoingLeft && GoingRight == false) // Going Left //
                {
                    ballsRigidbody.AddForce(leftish * BallsMass, ForceMode2D.Impulse);
                }
                else if (GoingRight && GoingLeft == false) // Going Right //
                {
                    ballsRigidbody.AddForce(rightish * BallsMass, ForceMode2D.Impulse);
                }
            }
        }
    }
}
