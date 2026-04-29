using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [Header("Optional - for faded transitions")]
    [SerializeField] private SceneFader sceneFader;

    [Header("Start Game Canvas")]
    [SerializeField] private CanvasGroup gameCanvas;
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private GameObject nextCanvasButton;
    [SerializeField] private float buttonAppearDelay = 3f;
    [SerializeField] private CanvasGroup nextCanvas;

    [Header("Decision UI")]
    [SerializeField] private GameObject blurImage;
    [SerializeField] private GameObject decisionButton1;
    [SerializeField] private GameObject decisionButton2;
    [SerializeField] private GameObject decisionButton3;
    [SerializeField] private GameObject decisionText;
    [SerializeField] private GameObject creditScreen;
    [SerializeField] private GameObject creditCloseButton;

    // ── Game flow ──────────────────────────────────────

    public void StartGame()
    {
        if (gameCanvas == null) return;
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
            nextCanvasButton.SetActive(true);
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

    public void NextScene()
    {
        int next = SceneManager.GetActiveScene().buildIndex + 1;
        if (next < SceneManager.sceneCountInBuildSettings)
        {
            if (sceneFader != null)
                sceneFader.FadeAndLoad(SceneUtility.GetScenePathByBuildIndex(next), 1f);
            else
                SceneManager.LoadScene(next);
        }
    }

    // Call this to show the decision UI
    public void Decision()
    {
        if (blurImage)   blurImage.SetActive(true);
        if (decisionButton1) decisionButton1.SetActive(true);
        if (decisionButton2) decisionButton2.SetActive(true);
        if (decisionButton3) decisionButton3.SetActive(true);
        if (decisionText) decisionText.SetActive(true);
    }

    // Wire this to each decision button's OnClick — set a different int per button in the Inspector
    public void MakeDecision(int scoreChange)
    {
        PlayerStats.Instance.AddEnvironmentScore(scoreChange);
        NextScene();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

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

    // ── UI toggles ─────────────────────────────────────

    public void ShowPanel(GameObject panel)  => panel.SetActive(true);
    public void HidePanel(GameObject panel)  => panel.SetActive(false);
    public void TogglePanel(GameObject panel) => panel.SetActive(!panel.activeSelf);


}
