using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Attach to the clue root GameObject.
// Spawns linePrefabs in order via Next button, then shows options.
// Choosing an option spawns optionAResultPrefab or optionBResultPrefab.
// Sets isDone = true after the result prefab appears.
public class SequentialDialogueClue : ClueBase
{
    [Header("UI References")]
    public Transform textContainer;
    public Button nextButton;

    [Header("Line Prefabs (one per dialogue line)")]
    public GameObject[] linePrefabs;

    [Header("Spawn Animation")]
    public float spawnAnimDuration = 0.25f;

    [Header("Options")]
    public GameObject optionsContainer;
    public Button optionAButton;
    public Button optionBButton;
    public string optionALabel;
    public string optionBLabel;

    [Header("Option Prefabs")]
    public GameObject optionAResponsePrefab;
    public GameObject optionAResultPrefab;
    public GameObject optionBResponsePrefab;
    public GameObject optionBResultPrefab;
    public float resultDelay = 1.5f;

    private int currentLine = 0;

    void Awake()
    {
        nextButton.onClick.AddListener(OnNextClicked);
        optionAButton.onClick.AddListener(() => OnOptionChosen(optionAResponsePrefab, optionAResultPrefab));
        optionBButton.onClick.AddListener(() => OnOptionChosen(optionBResponsePrefab, optionBResultPrefab));
    }

    void Start()
    {
        TMP_Text labelA = optionAButton.GetComponentInChildren<TMP_Text>();
        if (labelA != null) labelA.text = optionALabel;

        TMP_Text labelB = optionBButton.GetComponentInChildren<TMP_Text>();
        if (labelB != null) labelB.text = optionBLabel;
    }

    public override void OnClueOpen()
    {
        isDone = false;
        currentLine = 0;

        foreach (Transform child in textContainer)
            Destroy(child.gameObject);

        optionsContainer.SetActive(false);
        nextButton.gameObject.SetActive(true);

        SpawnPrefab(linePrefabs[currentLine]);
        currentLine++;

        if (currentLine >= linePrefabs.Length)
        {
            nextButton.gameObject.SetActive(false);
            optionsContainer.SetActive(true);
        }
    }

    void OnNextClicked()
    {
        if (currentLine >= linePrefabs.Length) return;

        SpawnPrefab(linePrefabs[currentLine]);
        currentLine++;

        if (currentLine >= linePrefabs.Length)
        {
            nextButton.gameObject.SetActive(false);
            optionsContainer.SetActive(true);
        }
    }

    void OnOptionChosen(GameObject responsePrefab, GameObject resultPrefab)
    {
        optionsContainer.SetActive(false);
        StartCoroutine(ShowResponseThenResult(responsePrefab, resultPrefab));
    }

    IEnumerator ShowResponseThenResult(GameObject responsePrefab, GameObject resultPrefab)
    {
        SpawnPrefab(responsePrefab);
        yield return new WaitForSeconds(resultDelay);
        SpawnPrefab(resultPrefab);
        isDone = true;
    }

    GameObject SpawnPrefab(GameObject prefab)
    {
        GameObject go = Instantiate(prefab, textContainer);

        if (textContainer.childCount > 2)
            Destroy(textContainer.GetChild(0).gameObject);

        StartCoroutine(GrowIn(go.GetComponent<RectTransform>()));
        return go;
    }

    IEnumerator GrowIn(RectTransform rt)
    {
        rt.localScale = new Vector3(1f, 0f, 1f);
        float elapsed = 0f;
        while (elapsed < spawnAnimDuration)
        {
            elapsed += Time.deltaTime;
            rt.localScale = new Vector3(1f, Mathf.Clamp01(elapsed / spawnAnimDuration), 1f);
            yield return null;
        }
        rt.localScale = Vector3.one;
    }
}
