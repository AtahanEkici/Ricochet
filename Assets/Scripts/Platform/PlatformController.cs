using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [Header("Controller Position")]
    [SerializeField] private Vector2 MousePosition = Vector2.zero;

    [Header("Movement")]
    [SerializeField] private Vector3 MovementVector = Vector3.zero;
    [SerializeField] private float MaxSpeed = 5f;
    [SerializeField] private float Platform_Move_Speed = 5f;
    [SerializeField] private float StopOffset = 0f;

    [Header("Local Components")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Foreign References")]
    [SerializeField] private Camera cam;
    [SerializeField] private WallGenerate WallGenerator;

    private void Awake()
    {
        GetLocalReferences();
    }
    private void Start()
    {
        GetForeignReferences();
    }
    private void FixedUpdate()
    {
        MovePlatformToMouseCoordinates();
    }
    private void Update()
    {
        SpeedCheck();
    }
    private void LateUpdate()
    {
        GetMousePosition();
    }
    private void SpeedCheck()
    {
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, MaxSpeed) ;
    }
    private void GetLocalReferences()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }
    private void GetForeignReferences()
    {
        cam = Camera.main;

        WallGenerator = WallGenerate.instance;

        float haltOfThePlatform = transform.localScale.y;
        float halfOfTheWall = WallGenerator.transform.GetChild(0).localScale.x / 2;
        float RightWallPos = WallGenerator.right.x;

        StopOffset = RightWallPos - (halfOfTheWall + haltOfThePlatform);
    }
    private void MovePlatformToMouseCoordinates()
    {
        if (!Input.GetMouseButton(0)) { return; }

        Vector2 pos = rb.position;

        if (pos != MousePosition)
        {
            MovementVector = Vector2.MoveTowards(new(pos.x, 0f), new(MousePosition.x, 0f), Platform_Move_Speed * Time.fixedDeltaTime);
            if(MovementVector.x < -StopOffset || MovementVector.x > StopOffset) { return; }
            rb.MovePosition(MovementVector);
        }
    }
    private void GetMousePosition()
    {
        MousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject CollidedObject = collision.gameObject;

        /*
        if(CollidedObject.CompareTag("Wall"))
        {
            
        }
        */

        Debug.Log("Collided: "+ CollidedObject.name+"");
    }
}
