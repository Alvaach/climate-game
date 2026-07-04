using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Fader")]
    [SerializeField] private SceneFader sceneFader;
    [SerializeField] private float preTransitionHold = 3f;

    [Header("Start Game-canvas")]
    [SerializeField] private CanvasGroup gameCanvas;
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private CanvasGroup nextCanvasButton;
    [SerializeField] private float buttonAppearDelay = 3f;
    [SerializeField] private CanvasGroup nextCanvas;

    [Header("Decision Prefabs (assign per scene)")]
    [SerializeField] private GameObject decisionAPrefab;
    [SerializeField] private GameObject decisionBPrefab;

    [Header("Decision UI")]
    [SerializeField] private GameObject blurImage;
    [SerializeField] private GameObject decisionButton1;
    [SerializeField] private GameObject decisionButton2;
    [SerializeField] private GameObject decisionButton3;
    [SerializeField] private GameObject pathButtonA;
    [SerializeField] private GameObject pathButtonB;
    [SerializeField] private GameObject decisionText;
    [SerializeField] private GameObject creditScreen;
    [SerializeField] private GameObject creditCloseButton;

    [Header("Decision Box Previews")]
    [SerializeField] private GameObject previewGreenBoxPrefab;
    [SerializeField] private GameObject previewRedBoxPrefab;
    [Tooltip("One container per option button, in the same order as the buttons")]
    [SerializeField] private Transform[] optionPreviewContainers;
    [Tooltip("Score value each option gives — controls box count and color (positive = green, negative = red)")]
    [SerializeField] private int[] optionScorePreviews;

    private MovementScript playerMovement;

    void Awake()
    {
        playerMovement = FindObjectOfType<MovementScript>();
    }

    // For starting scene
    public void StartGame()
    {
        if (gameCanvas == null) return;
        if (nextCanvasButton != null) { nextCanvasButton.alpha = 0f; nextCanvasButton.interactable = false; nextCanvasButton.blocksRaycasts = false; }
        gameCanvas.gameObject.SetActive(true);
        gameCanvas.alpha = 0f;
        StartCoroutine(FadeInThenShowButton(gameCanvas));
    }

    public void ShowNextCanvas()
    {
        if (nextCanvas == null) return;
        nextCanvas.gameObject.SetActive(true);
        nextCanvas.alpha = 0f;
        StartCoroutine(FadeIn(nextCanvas));
    }

    private IEnumerator FadeInThenShowButton(CanvasGroup cg)
    {
        yield return StartCoroutine(FadeIn(cg));
        yield return new WaitForSeconds(buttonAppearDelay);
        if (nextCanvasButton != null)
        {
            yield return StartCoroutine(FadeIn(nextCanvasButton));
            nextCanvasButton.interactable = true;
            nextCanvasButton.blocksRaycasts = true;
        }
    }

    private IEnumerator FadeIn(CanvasGroup cg)
    {
        float elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Clamp01(elapsed / fadeInDuration);
            yield return null;
        }
        cg.alpha = 1f;
    }

    // Long term decisions
        public void ChoosePathA(int scoreChange)
    {
        DisableButton(pathButtonA);
        DisableButton(pathButtonB);
        PathTracker.Instance?.ChoosePath(true);
        PlayerStats.Instance.AddEnvironmentScore(scoreChange);
        PlayerStats.Instance.SpawnStatsIcon(true);
        NextScene();
    }

    public void ChoosePathB(int scoreChange)
    {
        DisableButton(pathButtonA);
        DisableButton(pathButtonB);
        PathTracker.Instance?.ChoosePath(false);
        PlayerStats.Instance.AddEnvironmentScore(scoreChange);
        PlayerStats.Instance.SpawnStatsIcon(false);
        NextScene();
    }

    // Swap scene
    public void NextScene()
    {
        StartCoroutine(NextSceneDelayed());
    }

    private IEnumerator NextSceneDelayed()
    {
        yield return new WaitForSeconds(preTransitionHold);
        int next = SceneManager.GetActiveScene().buildIndex + 1;
        if (next < SceneManager.sceneCountInBuildSettings)
        {
            if (sceneFader != null)
                sceneFader.FadeAndLoad(SceneUtility.GetScenePathByBuildIndex(next), 1f);
            else
                SceneManager.LoadScene(next);
        }
    }

    // Universal decision logic
    public void Decision()
    {
        if (playerMovement) playerMovement.enabled = false;
        if (blurImage)   blurImage.SetActive(true);
        if (decisionButton1) decisionButton1.SetActive(true);
        if (decisionButton2) decisionButton2.SetActive(true);
        if (decisionButton3) decisionButton3.SetActive(true);
        if (decisionText) decisionText.SetActive(true);
    }

    // Add to decision-bbuttons + set positive/negative score
    public void MakeDecision(int scoreChange)
    {
        DisableDecisionButtons();
        ShowOptionPreviews();
        PlayerStats.Instance.AddEnvironmentScore(scoreChange);
        PathTracker.Instance?.SpawnNext();
        NextScene();
    }

    private void DisableDecisionButtons()
    {
        DisableButton(decisionButton1);
        DisableButton(decisionButton2);
        DisableButton(decisionButton3);
    }

    private void DisableButton(GameObject btn)
    {
        if (btn == null) return;
        if (btn.TryGetComponent<Button>(out var b))
            b.interactable = false;
    }

    private void ShowOptionPreviews()
    {
        if (optionPreviewContainers == null || optionScorePreviews == null) return;
        int count = Mathf.Min(optionPreviewContainers.Length, optionScorePreviews.Length);
        for (int i = 0; i < count; i++)
        {
            Transform container = optionPreviewContainers[i];
            int score = optionScorePreviews[i];
            if (container == null || score == 0) continue;

            GameObject prefab = score > 0 ? previewGreenBoxPrefab : previewRedBoxPrefab;
            if (prefab == null) continue;

            int boxCount = Mathf.Abs(score);
            for (int b = 0; b < boxCount; b++)
                Instantiate(prefab, container);
        }
    }

    public void UnlockStatsBox()
    {
        if (PlayerStats.Instance != null)
            PlayerStats.Instance.UnlockStatsBox();
    }

    public void RestartGame()
    {
        if (PlayerStats.Instance != null) PlayerStats.Instance.ResetStats();
        if (PathTracker.Instance != null) PathTracker.Instance.ResetPath();

        if (sceneFader != null)
            sceneFader.FadeAndLoad("StartingScene", 1f);
        else
            SceneManager.LoadScene("StartingScene");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // credits on starting screena
    public void CreditScreen()
    {
        if (creditScreen) creditScreen.SetActive(true);
        if (creditCloseButton) creditCloseButton.SetActive(true);
    }

    public void CloseCreditScreen()
    {
        if (creditScreen) creditScreen.SetActive(false);
        if (creditCloseButton) creditCloseButton.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame
            && creditScreen != null && creditScreen.activeSelf)
            CloseCreditScreen();
    }

    public void ShowPanel(GameObject panel)  => panel.SetActive(true);
    public void HidePanel(GameObject panel)  => panel.SetActive(false);
    public void TogglePanel(GameObject panel) => panel.SetActive(!panel.activeSelf);
}
