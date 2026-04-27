using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance;

    public bool hasReadLetter = false;

    private void Awake()
    {
        Instance = this;
    }
}