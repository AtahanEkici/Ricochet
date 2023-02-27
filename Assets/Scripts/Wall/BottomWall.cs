using UnityEngine;
public class BottomWall : MonoBehaviour
{
    [Header("Ball Info")]
    [SerializeField] private GameObject DestroyParticle;
    [SerializeField] private readonly string BallTag = "Ball";
    [SerializeField] private readonly string ParticlePath = "Particles/BallDestroy";

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
        DestroyParticle = Resources.Load<GameObject>(ParticlePath) as GameObject;
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
        GameObject Go = Instantiate(DestroyParticle, go.transform.position, go.transform.rotation);
        SetColorToParticle(Go);
        LevelManager.RemoveFromBalls(go);
        PlayDeathSound();
        CameraShake.Instance.InduceStress(DeathShaker);
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
