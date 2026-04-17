using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    //bootstrap?
    [SerializeField] private GameObject gameManagerPrefab;

    void Awake()
    {
        if (PlayerStats.Instance == null)
            Instantiate(gameManagerPrefab);
    }
}