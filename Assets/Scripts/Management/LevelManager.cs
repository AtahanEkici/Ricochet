using System.Collections.Generic;
using UnityEngine;
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Total Block Count")]
    [SerializeField] private List<GameObject> bricks;

    [Header("Total Ball Count")]
    [SerializeField] private List<GameObject> balls;
    private void Awake()
    {
        CheckInstance();
    }
    private void Start()
    {
        
    }
    public void AddToBalls(GameObject go)
    {
        if (go.GetComponent<BallController>() != null)
        {
            balls.Add(go);
        }
    }
    public void RemoveFromBalls(GameObject go)
    {
        int instance = go.GetInstanceID();

        for (int i = 0; i < balls.Count; i++)
        {
            if (balls[i].GetInstanceID() == instance)
            {
                balls.RemoveAt(i);
            }
        }
        CheckBalls();
    }
    public void AddToBricks(GameObject go)
    {
        if (go.GetComponent<BrickController>() != null)
        {
            bricks.Add(go);
        }
    }
    public void RemoveFromBricks(GameObject go)
    {
        int instance = go.GetInstanceID();

        for(int i=0;i<bricks.Count;i++)
        {
            if (bricks[i].GetInstanceID() == instance)
            {
                bricks.RemoveAt(i);
            }
        }
        CheckBricks();
    }
    private void RemoveAllFromBricks()
    {
        bricks.Clear();
    }
    private void RemoveallFromBalls()
    {
        balls.Clear();
    }
    private void CheckBricks()
    {
        if(bricks.Count <= 0 && balls.Count > 0) // Level Passed //
        {
            Debug.Log("Level Passed");
        }
    }
    private void CheckBalls()
    {
        if(balls.Count <= 0) // Game Over //
        {
            Debug.Log("Game Over");
        }
    }
    public int GetBrickCount()
    {
        return bricks.Count;
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
}
