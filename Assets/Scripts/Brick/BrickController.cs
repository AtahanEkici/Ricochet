using System;
using UnityEngine;
public class BrickController : MonoBehaviour
{
    private static readonly string BallTag = "Ball";
    private const string x2PowerUP = "PowerUp/x2";
    private const string div2PowerUP = "PowerUp/div2";
    private const string Plus1PowerUP = "PowerUp/+1";

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
    [Range(0, 100)]
    [SerializeField] private static float SpawnProbability = 5;

    [Header("PowerUps")]
    [SerializeField] private static GameObject x2;
    [SerializeField] private static GameObject div2;
    [SerializeField] private static GameObject plus1;

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
        x2 = Resources.Load<GameObject>(x2PowerUP);
        div2 = Resources.Load<GameObject>(div2PowerUP);
        plus1 = Resources.Load<GameObject>(Plus1PowerUP);
    }
    private void GetForeignReferences()
    {
        if(DestroyParticle == null)
        {
            DestroyParticle = Resources.Load<GameObject>(ParticlePath);
        } 
    }
    private static void SpawnPowerUp(Vector3 ObjectPosition)
    {
        int roll = UnityEngine.Random.Range(0, 100);

        if (roll >= SpawnProbability)
        {
            Debug.Log("No luck");
            return;
        }

        int selected = UnityEngine.Random.Range(1, 3);
        Debug.Log("Selected: "+selected);

        switch (selected)
        {
            case 1:
                Instantiate(x2, ObjectPosition, Quaternion.identity);
                break;

            case 2:
                Instantiate(div2, ObjectPosition, Quaternion.identity);
                break;

            case 3:
                /*
                Instantiate(plus1, ObjectPosition, Quaternion.identity);
                */
                break;
            
            default:
                Instantiate(x2, ObjectPosition, Quaternion.identity);
                break;
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
