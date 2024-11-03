using System.Collections.Generic;
using UnityEngine;

public class ExplodableCube : MonoBehaviour
{
    private readonly int MaxExplosionChance = 100;
    private readonly int MinCountToSpawn = 2;
    private readonly int MaxCountToSpawn = 6;
    private readonly int Divider = 2;

    [Header("Настройки куба")]
    [SerializeField, Range(5, 30)] private float _explosionRadius;
    [SerializeField, Range(100, 1000)] private float _explosionForce;

    private int _explosionChance;
    private Rigidbody _rigidbody;
    private MeshRenderer _renderer;

    public void SetExplosionChance(int parentExplosionChance)
    {
        _explosionChance = parentExplosionChance / Divider;
    }

    public void SetScale(Vector3 parentScale)
    {
        transform.localScale = parentScale / Divider;
    }

    public void SetMaterial(Material material)
    {
        _renderer.sharedMaterial = material;
    }

    public void AddExplosionForce(Vector3 explosionPosition)
    {
        _rigidbody.AddExplosionForce(_explosionForce, explosionPosition, _explosionRadius);
    }
   
    private void Awake()
    {
        if(transform.localScale.x == 1)
            _explosionChance = MaxExplosionChance;

        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<MeshRenderer>();
    }

    private void OnMouseUpAsButton()
    {
        int chanceToExploid = UserUtils.GetRandomNumber(MaxExplosionChance);

        if(chanceToExploid < _explosionChance)
        {
            int countToSpawn = UserUtils.GetRandomNumber(MinCountToSpawn, MaxCountToSpawn + 1);
            List<ExplodableCube> cubesToExploid = ExplodableCubesSpawner.Instance.GetSpawnedCubes(transform.position,
                transform.localScale, _explosionChance, countToSpawn);

            foreach (ExplodableCube cube in cubesToExploid)
                cube.AddExplosionForce(transform.position);
        }

        Destroy(gameObject);
    }
}
