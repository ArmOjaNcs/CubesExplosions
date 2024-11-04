using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshRenderer))]
public class ExplodableCube : MonoBehaviour
{
    [Header("��������� ����")]
    [SerializeField, Range(5, 30)] private float _explosionRadius;
    [SerializeField, Range(100, 1000)] private float _explosionForce;

    private Rigidbody _rigidbody;
    private MeshRenderer _renderer;

    private readonly int _maxExplosionChance = 100;
    private readonly int _minCountToSpawn = 2;
    private readonly int _maxCountToSpawn = 6;
    private readonly int _divider = 2;

    private ExplodableCubesSpawner _spawner;
    private int _explosionChance;

    private void Awake()
    {
        if(transform.localScale.x == 1)
        {
            _spawner = (ExplodableCubesSpawner)FindObjectOfType(typeof(ExplodableCubesSpawner));
            _explosionChance = _maxExplosionChance;
        }

        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<MeshRenderer>();
    }

    private void OnMouseUpAsButton()
    {
        int chanceToExploid = UserUtils.GetRandomNumber(_maxExplosionChance);

        if(chanceToExploid < _explosionChance)
        {
            int countToSpawn = UserUtils.GetRandomNumber(_minCountToSpawn, _maxCountToSpawn + 1);
            List<ExplodableCube> cubesToExploid = _spawner.GetSpawnedCubes(transform.position,
                transform.localScale, _explosionChance, countToSpawn, _spawner);

            foreach (ExplodableCube cube in cubesToExploid)
                cube.AddExplosionForce(transform.position);
        }

        Destroy(gameObject);
    }

    public void Init(int parentExplosionChance, Vector3 parentScale, Material material, ExplodableCubesSpawner spawner)
    {
        _explosionChance = parentExplosionChance / _divider;
        transform.localScale = parentScale / _divider;
        SetMaterial(material);
        _spawner = spawner;
    }

    public void SetMaterial(Material material)
    {
        _renderer.sharedMaterial = material;
    }

    public void AddExplosionForce(Vector3 explosionPosition)
    {
        _rigidbody.AddExplosionForce(_explosionForce, explosionPosition, _explosionRadius);
    }
}
