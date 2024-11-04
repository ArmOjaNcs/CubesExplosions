using UnityEngine;

public class MaterialHolder : MonoBehaviour
{
    [SerializeField] private Material[] _materials;
    
    public Material GetMaterial()
    {
        int numberOfMaterial = Random.Range(0, _materials.Length);
        return _materials[numberOfMaterial];
    }
}
