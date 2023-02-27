using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[DefaultExecutionOrder(-1000)]
public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;

    [Header("Total Block Count")]
    [SerializeField] private List<GameObject> bricks;

    [Header("Total Ball Count")]
    [SerializeField] private List<GameObject> balls;

    public static LevelManager Instance { get { return instance; } }
    private void Awake()
    {
        CheckInstance();
        bricks = new List<GameObject>();
        balls = new List<GameObject>();
    }

    private void Start()
    {

    }

    public static void AddToBalls(GameObject go)
    {
        if (go.GetComponent<BallController>() != null)
        {
            instance.balls.Add(go);
        }
    }

    public static void RemoveFromBalls(GameObject go)
    {
        int instanceID = go.GetInstanceID();

        for (int i = 0; i < instance.balls.Count; i++)
        {
            if (instance.balls[i].GetInstanceID() == instanceID)
            {
                instance.balls.RemoveAt(i);
            }
        }
        CheckBalls();
    }

    public static void AddToBricks(GameObject go)
    {
        if (go.GetComponent<BrickController>() != null)
        {
            instance.bricks.Add(go);
        }
    }

    public static void RemoveFromBricks(GameObject go)
    {
        int instanceID = go.GetInstanceID();

        for (int i = 0; i < instance.bricks.Count; i++)
        {
            if (instance.bricks[i].GetInstanceID() == instanceID)
            {
                instance.bricks.RemoveAt(i);
            }
        }
        CheckBricks();
    }

    private static void RemoveAllFromBricks()
    {
        instance.bricks.Clear();
    }

    private static void RemoveallFromBalls()
    {
        instance.balls.Clear();
    }

    private static void CheckBricks()
    {
        if (instance.bricks.Count <= 0 && instance.balls.Count > 0) // Level Passed //
        {
            Debug.Log("Level Passed");
        }
    }

    private static void CheckBalls()
    {
        if (instance.balls.Count <= 0) // Game Over //
        {
            Debug.Log("Game Over");
        }
    }

    public static int GetBrickCount()
    {
        return instance.bricks.Count;
    }

    public static int GetBallCount()
    {
        return instance.balls.Count;
    }

    private void CheckInstance()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void LoadLevel(string SceneName)
    {
        SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Single);
    }
}
