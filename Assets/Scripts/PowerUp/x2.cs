using UnityEngine;
public class x2 : MonoBehaviour
{
    private const string PlatformTag = "Platform";
    private const string WallTag = "Wall";

    [Header("Rigidbody2D")]
    [SerializeField] private Rigidbody2D rb2d;

    [Header("Size Growth Multiplier")]
    [SerializeField] private float SizeGrowthMultiplier = 1.5f;
    private void Awake()
    {
        GetReferences();
    }
    private void GetReferences()
    {
        if(rb2d == null)
        {
            rb2d = GetComponent<Rigidbody2D>();
        }
    }
    private void CollisionHandler(GameObject go)
    {
        GameObject Speaker = new();

        AudioSpawner AudioSpawn = Speaker.AddComponent<AudioSpawner>();
        AudioSpawn.isForButton = false;
        AudioSpawn.audio_Clip = AudioManager.PowerUpGain;

        Vector2 new_Scale = go.transform.localScale;
        new_Scale.y *= SizeGrowthMultiplier;
        go.transform.localScale = new_Scale;

        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(PlatformTag))
        {
            CollisionHandler(collision.gameObject);
        }
        else if(collision.CompareTag(WallTag))
        {
            Destroy(gameObject);
        }
    }
}
