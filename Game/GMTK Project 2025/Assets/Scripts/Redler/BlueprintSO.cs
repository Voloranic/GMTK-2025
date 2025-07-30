using UnityEngine;

[CreateAssetMenu(fileName = "BlueprintSO", menuName = "Scriptable Objects/BlueprintSO")]
public class BlueprintSO : ScriptableObject
{
    [SerializeField] private int id = 0;
    [SerializeField] private string _name = "Default Name";

    public int GetId()
    {
        return id;
    }

    public string GetName()
    {
        return _name;
    }
}
