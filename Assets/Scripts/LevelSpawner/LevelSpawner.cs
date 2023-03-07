using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[DefaultExecutionOrder(-100)]
public class LevelSpawner : MonoBehaviour
{
    public static LevelSpawner Instance { get; private set; }
    private LevelSpawner() { }

    [Header("Brick Resources")]
    [SerializeField] private const string BrickPath = "Bricks";

    [Header("Brick Info")]
    [SerializeField] private List<GameObject> Bricks;

    [Header("Wall Info")]
    [SerializeField] private WallGenerate wg;
    [SerializeField] private Vector3 WallScale = Vector3.zero;
    [SerializeField] private Vector3 X_Offset = new Vector3(0.5f,0.1f,0);

    [Header("Spawn Location")]
    [SerializeField] private Vector3 SpawnLocation = Vector3.zero;
    [SerializeField] private Vector3 CurrentSpawnPoint = Vector3.zero;

    [Header("Spawn Randomizing")]
    [Range(25,100)]
    [SerializeField] private int Percentage = 50;

    private void Awake()
    {
        CheckInstance();
        GetReferences();
    }
    private void OnEnable()
    {
        GetForeignReferences();
        SpawnObjects();
    }
    private void SpawnObjects()
    {
        int Rand = Random.Range(0, (Bricks.Count - 1));

        GameObject Master = new GameObject("Master");
        Master.transform.position = Vector3.zero;

        GameObject RandomObject = Bricks[Rand];

        Vector3 WallRight = wg.right;
        Vector3 TopLeft = wg.top + wg.left;
        Vector3 ObjectScale = RandomObject.transform.localScale;
        Vector3 WallScale = wg.WallObject.transform.localScale;

        SpawnLocation = new Vector3((TopLeft.x + (ObjectScale.x + WallScale.x)), (TopLeft.y - ObjectScale.x),0f);

        CurrentSpawnPoint = SpawnLocation + X_Offset;

        float DesiredOffset = WallRight.x - (ObjectScale.x + (WallScale.x / 2) + X_Offset.x);

        while (CurrentSpawnPoint.x < DesiredOffset)
        {
            if(Percentage >= Random.Range(0,100))
            {
                GameObject temp = Instantiate(RandomObject, CurrentSpawnPoint, RandomObject.transform.rotation);
                temp.transform.SetParent(Master.transform);
            }

            CurrentSpawnPoint = new Vector3(CurrentSpawnPoint.x + (ObjectScale.x * 2) + X_Offset.x, CurrentSpawnPoint.y, 0);
        }
    }
    private void GetReferences()
    {
            Bricks = Resources.LoadAll<GameObject>(BrickPath).ToList();
    }
    private void GetForeignReferences()
    {
        WallGenerate wg = WallGenerate.Instance;
    }
    private void CheckInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
