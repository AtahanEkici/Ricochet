using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[DefaultExecutionOrder(-1000)]
public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;

    [Header("Platform Operations")]
    [SerializeField] private const string PlatformPath = "Platform/Platform";
    [SerializeField] private GameObject Platform;

    [Header("Congradulations Particle")]
    [SerializeField] private const string BsParticlePath = "Particles/randombullshitgo";
    [SerializeField] private static GameObject CongradulationsParticle;

    [Header("Current Level")]
    [SerializeField] private static string LevelName = "";

    [Header("Max Level")]
    [SerializeField] private static int MaxLevel = 15;

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

        SceneManager.sceneUnloaded += OnSceneUnLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneUnLoaded(Scene scene)
    {
        RemoveAllFromBricks();
        RemoveAllFromBalls();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneLoad)
    {
        LevelName = scene.name;
        SpawnPlatformOnLevelLoad(scene);
    }
    private void SpawnPlatformOnLevelLoad(Scene scene)
    {
        if(scene.name == GameManager.StartMenuName) { return; }

        Platform = Resources.Load<GameObject>(PlatformPath) as GameObject;

        if(FindObjectsOfType<PlatformController>().Length <= 0)
        {
            Instantiate(Platform);
        }
    }
    public static Transform ReturnABall()
    {
        return instance.balls[0].transform;
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
    private void RemoveAllFromBalls()
    {
        instance.balls.Clear();
    }
    public Vector2 GetClosestBrickCoordinates(Vector2 ObjectLocation)
    {
        float min = float.MaxValue;
        int index = 0;

        for (int i = 0; i < bricks.Count; i++)
        {
            float distance = Vector2.Distance(ObjectLocation,(Vector2)bricks[i].transform.position);

            if (distance > min)
            {
                min = distance;
                index = i;
            }
        }
        return (Vector2)bricks[index].transform.position;
    }
    private static void CheckBricks()
    {
        if (instance.bricks.Count <= 0 && instance.balls.Count > 0) // Level Passed //
        {
            SpawnParticlesOnLevelPassed();
            GameManager.Instance.LevelPassed();
            Debug.Log("Level Passed");
        }
    }
    private static void CheckBalls()
    {
        if (instance.balls.Count <= 0) // Game Over //
        {
            Debug.Log("Game Over");
            GameManager.Instance.GameOver();
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
    public static void LevelPassed()
    {
        int level = int.Parse(LevelName);
        Debug.Log("Passed Level: " + level.ToString());
        level++;
        Debug.Log("loading Scene: " + level.ToString() + "");

        if(level > MaxLevel)
        {
            Debug.Log("Max Level Reached");
        }
        LoadLevel(level.ToString());
    }
    private static void SpawnParticlesOnLevelPassed()
    {
        try
        {
            BallController[] balls = FindObjectsOfType<BallController>();

            CongradulationsParticle = Resources.Load<GameObject>(BsParticlePath) as GameObject;

            for (int i = 0; i < balls.Length; i++)
            {
                Color ballColor = balls[i].gameObject.GetComponent<Renderer>().material.color;
                Instantiate(CongradulationsParticle,balls[i].transform.position,Quaternion.identity).GetComponent<ParticleSystem>().GetComponent<Renderer>().material.color = ballColor;
            } 
        }
        catch(Exception e)
        {
            Debug.Log(e.StackTrace + " >>> " + e.Message);
        }
    }
    public static void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
    public static void LoadLevel(string SceneName)
    {
        SceneManager.LoadSceneAsync(SceneName);
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnLoaded;
    }
}
