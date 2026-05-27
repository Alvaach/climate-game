using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameManagerPrefab;

    [Header("Test Overrides")]
    [SerializeField] private bool forceStatsBoxActive;
    [SerializeField] private bool forcePathChosen;
    [SerializeField] private bool forcedPathIsA = true;

    void Awake()
    {
        if (PlayerStats.Instance == null)
            Instantiate(gameManagerPrefab);

        if (forceStatsBoxActive)
            PlayerStats.Instance.UnlockStatsBox();

        if (forcePathChosen)
            PlayerStats.Instance.SpawnStatsIcon(forcedPathIsA);
    }
}