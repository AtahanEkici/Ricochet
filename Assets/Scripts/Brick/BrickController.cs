using System;
using UnityEngine;
public class BrickController : MonoBehaviour
{
    private readonly string BallTag = "Ball";

    [Header("Asset's Path")]
    [SerializeField] private static readonly string ParticlePath = "Particles/BrickDestroy";

    [Header("Foreign Components")]
    [SerializeField] private GameObject DestroyParticle;
    [SerializeField] private LevelManager level_manager;

    [Header("Local Components")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Health Info")]
    [SerializeField] private int health = 1;

    [Header("Point Info")]
    [SerializeField] private const int point = 1;
    [SerializeField] private int total_point = 1;

    [Header("CameraShake")]
    [SerializeField] private static Vector3 ShakeCoefficient = new(2.5f, 90, 0.3f);
    private void Awake()
    {
        GetLocalReferences();
    }
    private void OnEnable()
    {
        AddBricksToList();
    }
    private void Start()
    {
        GetForeignReferences();
    }
    private void AddBricksToList()
    {
        try
        {
            LevelManager.AddToBricks(gameObject);
        }
        catch (Exception e)
        {
            Debug.Log(e.StackTrace);
        }
    }
    public int GetHealth()
    {
        return health;
    }
    private void BeforeDestroy()
    {
        LevelManager.RemoveFromBricks(gameObject);
        GameObject Go = Instantiate(DestroyParticle, transform.position, Quaternion.identity);
        SetColorToParticle(Go);
        SliderController.UpdateSlider();
        ScoreManager.UpdateScore(total_point);
        CameraShake.Instance.InduceStress(ShakeCoefficient);
    }
    private void GetLocalReferences()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        total_point = health * point;
    }
    private void GetForeignReferences()
    {
        if(DestroyParticle == null)
        {
            DestroyParticle = Resources.Load<GameObject>(ParticlePath) as GameObject;
        } 
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
