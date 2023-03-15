using UnityEngine;
[DefaultExecutionOrder(-150)]
public class LevelSpawner : MonoBehaviour
{
    [Header("Brick Objects")]
    [SerializeField] private BrickController[] Bricks;

    [Header("Spawn Probability")]
    [SerializeField] private float probability = 0.5f;

    [Header("Max Objects")]
    [SerializeField] private int InitialObjectCount = 0;
    //[SerializeField] private int MaxObjectCount = 0;

    private void Awake()
    {
        GetBricks();
        RandomizeLevel();
    }
    private void GetBricks()
    {
        Bricks = GetComponentsInChildren<BrickController>();
        InitialObjectCount = Bricks.Length;
    }

    private void RandomizeLevel()
    {
        for(int i=0;i<Bricks.Length;i++)
        {
            if (probability >= Random.Range(0f, 1f))
            {
                Bricks[i].gameObject.SetActive(false);
            }
        } 
    }
}
