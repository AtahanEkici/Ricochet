using UnityEngine;
public class BrickController : MonoBehaviour
{
    private readonly string BallTag = "Ball";

    [Header("Foreign Componenets")]
    [SerializeField] private static readonly string ParticlePath = "Particles/BrickDestroy";
    [SerializeField] private GameObject DestroyParticle;

    [Header("Local Components")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Health Info")]
    [SerializeField] private int health = 1;
    private void Awake()
    {
        GetLocalReferences();
    }
    private void Start()
    {
        GetForeignReferences();
        LevelManager.Instance.AddToBricks(gameObject);
    }
    private void BeforeDestroy()
    {
        LevelManager.Instance.RemoveFromBricks(gameObject);
        GameObject Go = Instantiate(DestroyParticle, transform.position, Quaternion.identity);
        SetColorToParticle(Go);
    }
    private void GetLocalReferences()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }
    private void GetForeignReferences()
    {
        DestroyParticle = Resources.Load<GameObject>(ParticlePath) as GameObject;
    }
    private void Damage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            BeforeDestroy();
            Destroy(gameObject);
        }
    }
    private void SetColorToParticle(GameObject go, Color color)
    {
        ParticleSystem ps = go.GetComponent<ParticleSystem>();
        ps.GetComponent<Renderer>().material.color = color;
    }
    private void SetColorToParticle(GameObject go)
    {
        ParticleSystem ps = go.GetComponent<ParticleSystem>();
        ps.GetComponent<Renderer>().material.color = gameObject.GetComponent<Renderer>().material.color;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {     
        GameObject CollidedObject = collision.gameObject;

        if(CollidedObject.CompareTag(BallTag))
        {
            Damage(CollidedObject.GetComponent<BallController>().GetDamageNumber());
        }
    }
}
