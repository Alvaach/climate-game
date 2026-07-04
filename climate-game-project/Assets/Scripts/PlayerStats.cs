using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    public int globalScore = 0;

    [Header("Scene Setup")]
    [Tooltip("Player stays inactive in these scenes")]
    [SerializeField] private string[] playerInactiveScenes;

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

    void OnEnable()  => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (System.Array.IndexOf(playerInactiveScenes, scene.name) >= 0) return;

        MovementScript[] players = FindObjectsByType<MovementScript>(FindObjectsInactive.Include);
        if (players.Length > 0) players[0].gameObject.SetActive(true);
    }

    public void ResetStats()
    {
        globalScore = 0;
        statsBoxUnlocked = false;
        if (statsBoxCanvas != null)
        {
            StopAllCoroutines();
            statsBoxCanvas.alpha = 0f;
            statsBoxCanvas.gameObject.SetActive(false);
        }
        if (statsIconContainer != null)
        {
            foreach (Transform child in statsIconContainer)
                Destroy(child.gameObject);
        }
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

    public void FadeOutStatsBox()
    {
        if (statsBoxCanvas != null && statsBoxCanvas.gameObject.activeSelf)
            StartCoroutine(FadeOutStatsBoxCoroutine());
    }

    private IEnumerator FadeOutStatsBoxCoroutine()
    {
        float elapsed = 0f;
        float startAlpha = statsBoxCanvas.alpha;
        while (elapsed < statsBoxFadeDuration)
        {
            elapsed += Time.deltaTime;
            statsBoxCanvas.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / statsBoxFadeDuration);
            yield return null;
        }
        statsBoxCanvas.alpha = 0f;
        statsBoxCanvas.gameObject.SetActive(false);
    }
}
