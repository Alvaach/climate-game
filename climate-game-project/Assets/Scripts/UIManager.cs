using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Optional - for faded transitions")]
    [SerializeField] private SceneFader sceneFader;

    [Header("Decision UI")]
    [SerializeField] private GameObject blurImage;
    [SerializeField] private GameObject decisionButton1;
    [SerializeField] private GameObject decisionButton2;
    [SerializeField] private GameObject decisionButton3;
    [SerializeField] private GameObject decisionText;

    // ── Game flow ──────────────────────────────────────

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

    // ── UI toggles ─────────────────────────────────────

    public void ShowPanel(GameObject panel)  => panel.SetActive(true);
    public void HidePanel(GameObject panel)  => panel.SetActive(false);
    public void TogglePanel(GameObject panel) => panel.SetActive(!panel.activeSelf);


}
