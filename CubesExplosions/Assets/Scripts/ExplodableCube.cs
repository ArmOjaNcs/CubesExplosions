using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshRenderer))]
public class ExplodableCube : MonoBehaviour
{
    [Header("Настройки куба")]
    [SerializeField, Range(5, 30)] private float _explosionRadius;
    [SerializeField, Range(100, 1000)] private float _explosionForce;

    private Rigidbody _rigidbody;
    private MeshRenderer _renderer;

    public readonly int MaxExplosionChance = 100;

    private readonly int _minCountToSpawn = 2;
    private readonly int _maxCountToSpawn = 6;
    private readonly int _divider = 2;

    private ExplodableCubesSpawner _spawner;
    private int _explosionChance;

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
            List<ExplodableCube> cubesToExploid = _spawner.GetSpawnedCubes(transform.position,
                transform.localScale, _explosionChance, countToSpawn, _spawner);

            foreach (ExplodableCube cube in cubesToExploid)
                cube.AddExplosionForce(transform.position);
        }

        Destroy(gameObject);
    }

    public void Init(int parentExplosionChance, Vector3 parentScale, Material material, ExplodableCubesSpawner spawner, bool isStart = false)
    {
        if (isStart)
            _explosionChance = MaxExplosionChance;
        else
        {
            _explosionChance = parentExplosionChance / _divider;
            transform.localScale = parentScale / _divider;
        }

        _spawner = spawner;
        _renderer.sharedMaterial = material;
    }

    public void AddExplosionForce(Vector3 explosionPosition)
    {
        _rigidbody.AddExplosionForce(_explosionForce, explosionPosition, _explosionRadius);
    }
}
