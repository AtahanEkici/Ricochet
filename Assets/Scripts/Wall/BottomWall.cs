using UnityEngine;
public class BottomWall : MonoBehaviour
{
    private readonly string BallTag = "Ball";
    private readonly string DestroyParticlePath = "Particles/BallDestroy";
    private readonly string FlameParticlePath = "Particles/Flame";

    [Header("Ball Info")]
    [SerializeField] private GameObject DestroyParticle;
    [SerializeField] private GameObject FlameParticle;

    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSource;

    [Header("Camera Shake")]
    [SerializeField] Vector3 DeathShaker = new(100, 90, 0.5f);
    private void Start()
    {
        GetLocalReferences();
        GetForeignReferences();
    }
    private void GetLocalReferences()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.ignoreListenerPause = true;
    }
    private void GetForeignReferences()
    {
        DestroyParticle = Resources.Load<GameObject>(DestroyParticlePath) as GameObject;
        FlameParticle = Resources.Load<GameObject>(FlameParticlePath) as GameObject;
    }
    private void PlayDeathSound()
    {
        audioSource.clip = AudioManager.Explosion;
        audioSource.Play();
    }
    private void SetColorToParticle(GameObject go)
    {
        ParticleSystem ps = go.GetComponent<ParticleSystem>();
        ps.GetComponent<Renderer>().material.color = gameObject.GetComponent<Renderer>().material.color;
    }
    private void BeforeDestroy(GameObject go)
    {
        PlayDeathSound();
        GameObject DestoroyedParticle = Instantiate(DestroyParticle, go.transform.position, go.transform.rotation);
        SetColorToParticle(DestoroyedParticle);
        LevelManager.RemoveFromBalls(go);
        CameraShake.Instance.InduceStress(DeathShaker);
        Instantiate(FlameParticle, go.transform.position, go.transform.rotation);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObject = collision.gameObject;

        if(collidedObject.CompareTag(BallTag))
        {
            BeforeDestroy(collidedObject);
            Destroy(collidedObject);
        }
    }
}
