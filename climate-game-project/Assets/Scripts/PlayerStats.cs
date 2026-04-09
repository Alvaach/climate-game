using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    public int globalScore = 0;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddEnvironmentScore(int amount)
    {
        globalScore += amount;
        if (VisualTracker.Instance != null) VisualTracker.Instance.AddVisual(amount);
    }
}
