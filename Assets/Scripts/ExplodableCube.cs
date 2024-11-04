using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshRenderer))]
public class ExplodableCube : MonoBehaviour
{
    public readonly int MaxExplosionChance = 100;
    
    private Rigidbody _rigidbody;
    private MeshRenderer _renderer;

    private readonly int _minCountToSpawn = 2;
    private readonly int _maxCountToSpawn = 6;
    private readonly int _divider = 2;

    private ExplodableCubesSpawner _spawner;
    private Exploder _exploder;
    private int _explosionChance;

    public Rigidbody Rigidbody => _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<MeshRenderer>();
    }

    private void OnMouseUpAsButton()
    {
        int chanceToExploid = Random.Range(0, MaxExplosionChance);
   
        if (chanceToExploid < _explosionChance)
        {
            int countToSpawn = Random.Range(_minCountToSpawn, _maxCountToSpawn + 1);
            _exploder.ExploidLikeParent(_spawner.SpawnCubes(transform.position,
                transform.localScale, _explosionChance, countToSpawn, _rigidbody.mass), this);
        }
        else
        {
            _exploder.ExploidLikeSingle(transform.position, transform.localScale.x);
        }

        Destroy(gameObject);
    }

    public void Init(int parentExplosionChance, Vector3 parentScale, Material material, 
        ExplodableCubesSpawner spawner, Exploder exploder, float mass, bool isStart = false)
    {
        if (isStart)
        {
            _explosionChance = MaxExplosionChance;
            _rigidbody.mass = mass;
        }
        else
        {
            _explosionChance = parentExplosionChance / _divider;
            transform.localScale = parentScale / _divider;
            _rigidbody.mass = mass / _divider;
        }

        _spawner = spawner;
        _exploder = exploder;
        _renderer.sharedMaterial = material;
    }
}
