using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    public int globalScore = 0;

    [Header("Stats Box")]
    [SerializeField] private CanvasGroup statsBoxCanvas;
    [SerializeField] private float statsBoxFadeDuration = 1f;
    [SerializeField] private Transform statsIconContainer;
    [SerializeField] private GameObject icon1Prefab;
    [SerializeField] private GameObject icon2Prefab;
    private bool statsBoxUnlocked = false;

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

    public void SpawnStatsIcon(bool isPathA)
    {
        if (statsIconContainer == null) return;
        GameObject prefab = isPathA ? icon1Prefab : icon2Prefab;
        if (prefab != null)
            Instantiate(prefab, statsIconContainer);
    }

    public void UnlockStatsBox()
    {
        if (statsBoxUnlocked) return;
        statsBoxUnlocked = true;
        if (statsBoxCanvas != null)
            StartCoroutine(FadeInStatsBox());
    }

    private IEnumerator FadeInStatsBox()
    {
        statsBoxCanvas.gameObject.SetActive(true);
        statsBoxCanvas.alpha = 0f;
        float elapsed = 0f;
        while (elapsed < statsBoxFadeDuration)
        {
            elapsed += Time.deltaTime;
            statsBoxCanvas.alpha = Mathf.Clamp01(elapsed / statsBoxFadeDuration);
            yield return null;
        }
        statsBoxCanvas.alpha = 1f;
    }
}
