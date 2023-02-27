using System;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [Header("Draw Debugging Rays")]
    [SerializeField] private bool isDebugging = false;
    [SerializeField] private float RayHeight = 10f;

    [Header("Controller Position")]
    [SerializeField] private Vector2 MousePosition = Vector2.zero;

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

    [Header("SmoothDeltaTime")]
    [SerializeField] private float smoothFixedDeltaTime = 0f;
    [SerializeField] private int numFrames = 10;
    [SerializeField] private float[] deltaTimeArray;
    [SerializeField] private int index;

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
        CalculateSmoothDeltaTime();
        MovePlatformToMouseCoordinates();
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
        if (!Input.GetMouseButton(0)) { return; }

        Vector2 pos = rb.position;

        if (pos != MousePosition)
        {
            MovementVector = Vector2.MoveTowards(new(pos.x, 0f), new(MousePosition.x, 0f), Platform_Move_Speed * smoothFixedDeltaTime);
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
    }
}
