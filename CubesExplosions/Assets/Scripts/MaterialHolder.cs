using UnityEngine;

public class MaterialHolder : MonoBehaviour
{
    [SerializeField] private Material[] _materials;

    public static MaterialHolder Instance { get; private set; }
    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            return;
        }

        Destroy(gameObject);
    }
    
    public Material GetMaterial()
    {
        int numberOfMaterial = UserUtils.GetRandomNumber(_materials.Length);
        return _materials[numberOfMaterial];
    }
}
