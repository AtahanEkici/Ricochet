using UnityEngine;
using UnityEngine.SceneManagement;
public class SpawnBallOnLevelLoad : MonoBehaviour
{
    [Header("Is Game Started ?")]
    [SerializeField] private bool IsStarted = false;

    [Header("Platform Controller")]
    [SerializeField] private PlatformController pc;

    [Header("Ball Variables")]
    [SerializeField] private GameObject Ball;
    [SerializeField] private GameObject InstantiatedBall;
    [SerializeField] private const string BallPath = "Ball/Ball";
    [SerializeField] private Collider2D BallCollider;

    private void Awake()
    {
        GetResources();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneLoad)
    {
        SpawnBall(scene);
    }
    private void Update()
    {
        StartGame();
    }
    private void SpawnBall(Scene scene)
    {
        if (scene.name == GameManager.StartMenuName) { return; }

        Vector3 PlatformPosition = transform.position;
        Vector3 DesiredPosition = new Vector3((PlatformPosition.x), (PlatformPosition.y + transform.localScale.x), PlatformPosition.z);
        InstantiatedBall = Instantiate(Ball, DesiredPosition, Quaternion.identity);
        GameManager.PauseGame();
        BallCollider = InstantiatedBall.GetComponent<Collider2D>();
        BallCollider.enabled = false;
    }
    private void GetResources()
    {
        if(Ball == null)
        {
            Ball = Resources.Load<GameObject>(BallPath) as GameObject;
        }
        if(pc == null)
        {
            pc = GetComponent<PlatformController>();
        }
    }
    private void StartGame()
    {
        if(SceneManager.GetActiveScene().name == GameManager.StartMenuName) { return; }

        if (IsStarted) { return; }

        if (Input.GetMouseButtonDown(0) || pc.AutoPilot)
        {
            IsStarted = true;
            Rigidbody2D ball_rigidbody = InstantiatedBall.GetComponent<Rigidbody2D>();
            ball_rigidbody.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
            GameManager.ResumeGame();
            BallCollider.enabled = true;
        }
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
