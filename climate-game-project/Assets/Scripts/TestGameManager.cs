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
        bool isDirectTest = PlayerStats.Instance == null;

        if (isDirectTest)
            Instantiate(gameManagerPrefab);

        if (forceStatsBoxActive)
            PlayerStats.Instance.UnlockStatsBox();

        // Only force path when launching a level directly in the editor (not from StartingScene)
        if (forcePathChosen && isDirectTest)
        {
            PathTracker.Instance?.ChoosePath(forcedPathIsA);
            PlayerStats.Instance.SpawnStatsIcon(forcedPathIsA);
        }
    }
}