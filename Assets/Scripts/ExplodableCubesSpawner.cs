using System.Collections.Generic;
using UnityEngine;

public class ExplodableCubesSpawner : MonoBehaviour
{
    [SerializeField] private ExplodableCube _explodableCubePrefab;
    [SerializeField, Range(1,7)] private int _startCubesCount;
    [SerializeField] private MaterialHolder _materialHolder;
    [SerializeField] private Exploder _exploder;
    [SerializeField, Range(1,2)] private float _startMass;

    private void Start()
    {
        SetStartCubes();
    }

    public List<ExplodableCube> SpawnCubes(Vector3 position, Vector3 scale, int explosionChance, int count, float mass)
    {
        List<ExplodableCube> explodableCubes = new List<ExplodableCube>();

        for(int i = 0; i < count; ++i)
        {
            var instantiatedCube = Instantiate(_explodableCubePrefab, position + GetPosition(i, scale), Quaternion.identity);
            explodableCubes.Add(instantiatedCube);
        }

        foreach(ExplodableCube explodableCube in explodableCubes)
            explodableCube.Init(explosionChance, scale, _materialHolder.GetMaterial(), this, _exploder, mass, false);

        return explodableCubes;
    }

    private Vector3 GetPosition(int cubeNumber, Vector3 scale, bool isStart = false)
    {
        float currentScale = scale.x;

        if (isStart == false)
            currentScale /= 2;

        switch(cubeNumber)
        {
            case 0:
                return new Vector3(currentScale, 0, 0);

            case 1:
                return new Vector3(-currentScale, 0, 0);

            case 2:
                return new Vector3(0, 0, currentScale);

            case 3:
                return new Vector3(0, 0, -currentScale);

            case 4:
                return new Vector3(currentScale, 0, currentScale);

            case 5:
                return new Vector3(-currentScale, 0, -currentScale);

            default:
                return Vector3.zero;
        }
    }

    private void SetStartCubes()
    {
        List<ExplodableCube> startCubes = new List<ExplodableCube>();
        var startCube = Instantiate(_explodableCubePrefab, transform.position, Quaternion.identity);
        int cubesCount = _startCubesCount;
        cubesCount--;
        startCubes.Add(startCube);

        for(int i = 0; i < cubesCount; i++)
        {
            var cube = Instantiate(_explodableCubePrefab, GetPosition(i, startCube.transform.localScale, true), Quaternion.identity);
            startCubes.Add(cube);
        }

        foreach (var cube in startCubes)
            cube.Init(cube.MaxExplosionChance, cube.transform.localScale, _materialHolder.GetMaterial(), this, _exploder, _startMass, true);
    }
}
