using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager Instance { get; private set; }

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseMenuPanel;

    [Header("Settings")]
    [Tooltip("Build index of the scene where the pause menu should NOT appear")]
    [SerializeField] private int disabledSceneIndex = 0;

    private bool isPaused = false;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Update()
    {
        if (Keyboard.current == null) return;
        if (!Keyboard.current.escapeKey.wasPressedThisFrame) return;
        if (SceneManager.GetActiveScene().buildIndex == disabledSceneIndex) return;

        if (isPaused)
            ClosePauseMenu();
        else
            OpenPauseMenu();
    }

    public void OpenPauseMenu()
    {
        if (pauseMenuPanel == null) return;
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenuPanel.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        if (pauseMenuPanel == null) return;
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenuPanel.SetActive(false);
    }

    public void RestartGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) player.SetActive(false);

        if (PlayerStats.Instance != null) Destroy(PlayerStats.Instance.gameObject);
        if (PathTracker.Instance != null) Destroy(PathTracker.Instance.gameObject);

        SceneFader fader = FindObjectOfType<SceneFader>();
        if (fader != null)
            fader.FadeAndLoad("StartingScene", 1f);
        else
            SceneManager.LoadScene("StartingScene");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
