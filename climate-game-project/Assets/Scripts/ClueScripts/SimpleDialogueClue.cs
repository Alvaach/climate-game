using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SimpleDialogueClue : ClueBase
{
    public Transform textContainer;
    public Button nextButton;

    [Header("Line Prefabs")]
    public GameObject[] linePrefabs;

    public float spawnAnimDuration = 0.25f;

    private int currentLine = 0;

    void Awake()
    {
        nextButton.onClick.AddListener(OnNextClicked);
    }

    public override void OnClueOpen()
    {
        isDone = false;
        currentLine = 0;

        foreach (Transform child in textContainer)
            Destroy(child.gameObject);

        nextButton.gameObject.SetActive(true);

        SpawnPrefab(linePrefabs[currentLine]);
        currentLine++;

        if (currentLine >= linePrefabs.Length)
        {
            nextButton.gameObject.SetActive(false);
            isDone = true;
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
            isDone = true;
        }
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
