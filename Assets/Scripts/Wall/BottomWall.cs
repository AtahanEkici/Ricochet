using UnityEngine;
public class BottomWall : MonoBehaviour
{
    [Header("Ball Info")]
    [SerializeField] private GameObject DestroyParticle;
    [SerializeField] private readonly string BallTag = "Ball";
    [SerializeField] private readonly string ParticlePath = "Particles/BallDestroy";
    private void Start()
    {
        GetForeignReferences();
    }
    private void GetForeignReferences()
    {
        DestroyParticle = Resources.Load<GameObject>(ParticlePath) as GameObject;
    }
    private void SetColorToParticle(GameObject go)
    {
        ParticleSystem ps = go.GetComponent<ParticleSystem>();
        ps.GetComponent<Renderer>().material.color = gameObject.GetComponent<Renderer>().material.color;
        ps = null; // Garbage Collector //
    }
    private void BeforeDestroy()
    {
        GameObject Go = Instantiate(DestroyParticle, transform.position, Quaternion.identity);
        SetColorToParticle(Go);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObject = collision.gameObject;

        if(collidedObject.CompareTag(BallTag))
        {
            BeforeDestroy();
            Destroy(collidedObject);
        }
    }
}
