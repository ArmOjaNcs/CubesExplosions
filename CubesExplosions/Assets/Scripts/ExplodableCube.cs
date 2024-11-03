using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ExplodableCube : MonoBehaviour
{
    private readonly int _maxExplosionChance = 100;
    private readonly int _minCountToSpawn = 2;
    private readonly int _maxCountToSpawn = 6;
    private readonly int _divider = 2;

    [Header("Настройки куба")]
    [SerializeField, Range(5, 30)] private float _explosionRadius;
    [SerializeField, Range(100, 1000)] private float _explosionForce;

    private int _explosionChance;
    private Rigidbody _rigidbody;
    private MeshRenderer _renderer;
   
    private void Awake()
    {
        if(transform.localScale.x == 1)
            _explosionChance = _maxExplosionChance;

        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<MeshRenderer>();
    }

    private void OnMouseUpAsButton()
    {
        int chanceToExploid = UserUtils.GetRandomNumber(_maxExplosionChance);

        if(chanceToExploid < _explosionChance)
        {
            int countToSpawn = UserUtils.GetRandomNumber(_minCountToSpawn, _maxCountToSpawn + 1);
            List<ExplodableCube> cubesToExploid = ExplodableCubesSpawner.Instance.GetSpawnedCubes(transform.position,
                transform.localScale, _explosionChance, countToSpawn);

            foreach (ExplodableCube cube in cubesToExploid)
                cube.AddExplosionForce(transform.position);
        }

        Destroy(gameObject);
    }

    public void SetExplosionChance(int parentExplosionChance)
    {
        _explosionChance = parentExplosionChance / _divider;
    }

    public void SetScale(Vector3 parentScale)
    {
        transform.localScale = parentScale / _divider;
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
