using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue Piece", menuName = "New Dialogue")]
public class DialogueSO : ScriptableObject
{
    [TextArea(3, 10)]
    public string[] sentences;
}
