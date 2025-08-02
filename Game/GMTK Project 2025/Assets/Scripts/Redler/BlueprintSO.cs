using UnityEngine;

[CreateAssetMenu(fileName = "BlueprintSO", menuName = "Scriptable Objects/BlueprintSO")]
public class BlueprintSO : ScriptableObject
{
    [SerializeField] private int id = 0;
    [SerializeField] private string _name = "Default Name";

    [SerializeField] private float useDistance = 5f;
    [SerializeField] private LayerMask usableLayers;

    [SerializeField] private Sprite sprite;

    [SerializeField] private Vector2 spriteSize = Vector2.one;

    [SerializeField] private float useCooldown = 1f;

    [SerializeField] private GameObject customPrefab;

    [SerializeField] private AudioVariable[] useAudios = new AudioVariable[1];

    [SerializeField] private string useableTag;

    public int GetId()
    {
        return id;
    }

    public string GetName()
    {
        return _name;
    }

    public float GetUseDistance()
    {
        return useDistance;
    }

    public LayerMask GetUsableLayers()
    {
        return usableLayers;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }

    public float GetUseCooldown()
    {
        return useCooldown;
    }

    public Vector2 GetSpriteSize()
    {
        return spriteSize;
    }

    public GameObject GetCustomPrefab()
    {
        return customPrefab;
    }
    
    public AudioVariable[] GetUseAudios()
    {
        return useAudios;
    }
    public AudioVariable GetRandomUseAudio()
    {
        int random = Random.Range(0, useAudios.Length);
        return useAudios[random];
    }

    public string GetUsableTag()
    {
        return useableTag;
    }
}
