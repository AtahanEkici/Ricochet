using UnityEngine;
public class WallGenerate : MonoBehaviour
{
    public static WallGenerate instance = null;

    [Header("Draw Debugging Rays")]
    [SerializeField] private bool DebugMode = false;

    [Header("Foreign References")]
    [SerializeField] private Camera cam;

    [Header("Camera Bounds")]
    [SerializeField] public Vector3 left = Vector3.zero;
    [SerializeField] public Vector3 right = Vector3.zero;
    [SerializeField] private Vector3 top = Vector3.zero;
    [SerializeField] private Vector3 bottom = Vector3.zero;

    [Header("Walls Info")]
    [SerializeField] private GameObject[] Walls = new GameObject[4];
    [SerializeField] private GameObject WallObject = null;
    [SerializeField] private float LeftDistance = 5f;
    [SerializeField] private float TopDistance = 10f;
    private void Awake()
    {
        CheckInstance();
        GetLocalReferences();
    }
    private void Start()
    {
        GetForeignRefrences(); // Get Foreign References in Start //
        GetOrthographicBounds(); // Get Camera Boundaries //
        CalculateBounds();
        GenerateWalls();
    }
    private void CheckInstance()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void GetForeignRefrences()
    {
        if(cam == null)
        {
            cam = Camera.main;
        }  
    }
    private void GetLocalReferences()
    {
        if(WallObject == null)
        {
            WallObject = transform.GetChild(0).gameObject;
        } 
    }
    private void CalculateBounds() // Draw Debugging Rays //
    {
        Vector3 topleft = top + left;
        TopDistance = Vector3.Distance(topleft, top);
        LeftDistance = Vector3.Distance(topleft, left);
        if (DebugMode)
        {
            Vector3 center = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f));
            Vector3 topright = top + right;
            Vector3 bottomleft = bottom + left;
            Vector3 bottomright = bottom + right;

            Ray TopRay = new(topleft, top - topleft);
            Ray LeftRay = new(topleft, left - topleft);
            Ray BottomRay = new(topleft, top - topleft);
            Ray RightRay = new(topleft, left - topleft);

            Debug.DrawRay(center, top, Color.blue);
            Debug.DrawRay(center, bottom, Color.red);
            Debug.DrawRay(center, left, Color.yellow);
            Debug.DrawRay(center, right, Color.magenta);

            Debug.DrawRay(center, topleft, Color.cyan);
            Debug.DrawRay(center, topright, Color.white);
            Debug.DrawRay(center, bottomleft, Color.green);
            Debug.DrawRay(center, bottomright, Color.black);

            Debug.DrawRay(TopRay.origin, TopRay.direction * (TopDistance * 2f), Color.blue,Mathf.Infinity); // TOP viewport ray //
            Debug.DrawRay(LeftRay.origin, LeftRay.direction * (LeftDistance * 2f), Color.yellow, Mathf.Infinity); // LEFT viewport ray //
        }
    }
    private void GenerateWalls()
    {
        GameObject LeftWall = WallObject;
        LeftWall.transform.position = left;
        LeftWall.transform.localScale = new Vector3(LeftWall.transform.localScale.x, LeftDistance, LeftWall.transform.localScale.z);
        LeftWall.name = "LeftWall";
        Walls[0] = LeftWall;

        GameObject RightWal = Instantiate(WallObject, right, Quaternion.Euler(Vector3.zero));
        RightWal.transform.localScale = new Vector3(RightWal.transform.localScale.x, LeftDistance, RightWal.transform.localScale.z);
        RightWal.transform.SetParent(transform);
        RightWal.name = "RightWal";
        Walls[1] = RightWal;

        GameObject TopWall = Instantiate(WallObject, top, Quaternion.Euler(0,0,90));
        TopWall.transform.localScale = new Vector3(TopWall.transform.localScale.x, TopDistance, TopWall.transform.localScale.z);
        TopWall.transform.SetParent(transform);
        TopWall.name = "TopWall";
        Walls[2] = TopWall;

        GameObject BottomWall = Instantiate(WallObject, bottom, Quaternion.Euler(0, 0, 90));
        BottomWall.transform.localScale = new Vector3(BottomWall.transform.localScale.x, TopDistance, BottomWall.transform.localScale.z);
        BottomWall.transform.SetParent(transform);
        BottomWall.name = "BottomWall";
        BottomWall.AddComponent<BottomWall>();
        BottomWall.AddComponent<AudioSource>();
        Walls[3] = BottomWall;
    }
    private void GetOrthographicBounds() // Get Camera Bounds //
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = cam.orthographicSize * 2;
        Bounds bounds = new(cam.transform.position, new Vector3(cameraHeight * screenAspect, cameraHeight, 0));

        left = bounds.center - new Vector3(bounds.extents.x, 0, 0);
        right = bounds.center + new Vector3(bounds.extents.x, 0, 0);
        top = bounds.center + new Vector3(0, bounds.extents.y, 0);
        bottom = bounds.center - new Vector3(0, bounds.extents.y, 0);
    }
}
