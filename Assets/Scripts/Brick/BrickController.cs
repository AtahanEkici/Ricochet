using System;
using UnityEngine;
public class BrickController : MonoBehaviour
{
    private static readonly string BallTag = "Ball";
    private const string PowerUpResourcePath = "PowerUp/x2PowerUp";

    [Header("Asset's Path")]
    [SerializeField] private static readonly string ParticlePath = "Particles/BrickDestroy";

    [Header("Foreign Components")]
    [SerializeField] private static GameObject DestroyParticle;

    [Header("Health Info")]
    [SerializeField] private int health = 1;

    [Header("Point Info")]
    [SerializeField] private const int point = 1;
    [SerializeField] private int total_point = 1;

    [Header("CameraShake")]
    [SerializeField] private static Vector3 ShakeCoefficient = new(2.5f, 90, 0.3f);

    [Header("PowerUp Options")]
    [Range(0f, 100f)]
    [SerializeField] private static float SpawnProbability = 1f;

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
        SpawnPowerUp(transform.position);
    }
    private void GetLocalReferences()
    {
        total_point = health * point;
    }
    private void GetForeignReferences()
    {
        if(DestroyParticle == null)
        {
            DestroyParticle = Resources.Load<GameObject>(ParticlePath) as GameObject;
        } 
    }
    private static void SpawnPowerUp(Vector3 ObjectPosition)
    {
        if(UnityEngine.Random.Range(0f,100f) > SpawnProbability) { Debug.Log("No luck"); return; }

        GameObject Powerup = Resources.Load<GameObject>(PowerUpResourcePath) as GameObject;

        Instantiate(Powerup, ObjectPosition, Quaternion.identity);
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
