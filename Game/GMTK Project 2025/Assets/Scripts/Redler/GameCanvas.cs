using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    //For later use

    public static GameCanvas Instance;

    private void Awake()
    {
        Instance = this;
    }

}
