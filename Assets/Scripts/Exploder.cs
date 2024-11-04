using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    [Header("Настройки взрывателя")]
    [SerializeField, Range(5, 30)] private float _explosionRadius;
    [SerializeField, Range(100, 1000)] private float _explosionForce;
    [SerializeField, Range(1, 30)] private float _reducedForceMultiplier;

    private float _multiplierCoefficient;
    private float _multipliedRadius => _explosionRadius * _multiplierCoefficient;
    private float _multipliedForce => _explosionForce * _multiplierCoefficient;

    private void Awake()
    {
        _multiplierCoefficient = 1f;
    }

    public void ExploidLikeParent(List<ExplodableCube> cubesToExploid, ExplodableCube parentCube)
    {
        float scaleX = parentCube.transform.localScale.x;
        SetMultiplierCoefficient(scaleX);
        Vector3 explosionPosition = parentCube.transform.position;
        ExploidWithCoefficient(cubesToExploid, explosionPosition);
    }

    public void ExploidLikeSingle(Vector3 explosionPosition, float scaleX)
    {
        SetMultiplierCoefficient(scaleX);
        Collider[] hits = Physics.OverlapSphere(explosionPosition, _multipliedRadius);
        List<Rigidbody> rigidbodies = new List<Rigidbody>();

        foreach (Collider hit in hits)
            if(hit.attachedRigidbody != null)
                rigidbodies.Add(hit.attachedRigidbody);

        foreach(Rigidbody rigidbody in rigidbodies)
        {
            Vector3 offset = explosionPosition - rigidbody.transform.position;
            float reducedForce = offset.magnitude * _reducedForceMultiplier;
            rigidbody.AddExplosionForce(_multipliedForce - reducedForce, explosionPosition, _multipliedRadius);
        }
    }

    private void SetMultiplierCoefficient(float scaleX)
    {
        _multiplierCoefficient = 1.0f + (1.0f - scaleX);
    }

    private void ExploidWithCoefficient(List<ExplodableCube> cubesToExploid, Vector3 explosionPosition)
    {
        foreach (ExplodableCube cube in cubesToExploid)
            cube.Rigidbody.AddExplosionForce(_multipliedForce, explosionPosition, _multipliedRadius);
    }
}
