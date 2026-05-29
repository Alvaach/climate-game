using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingSceneSequence : MonoBehaviour
{
    [Header("Outcome Objects")]
    [SerializeField] private GameObject preOutcomeObject;
    [SerializeField] private GameObject outcomeA;
    [SerializeField] private GameObject outcomeB;
    [SerializeField] private GameObject iconA;
    [SerializeField] private GameObject iconB;
    [SerializeField] private GameObject postOutcomeObject;

    [Header("Spawn Sequence")]
    [SerializeField] private GameObject[] sequenceObjects;
    [SerializeField] private float timeBetweenSpawns = 1f;
    [SerializeField] private float fadeDuration = 0.8f;

    [Header("Ending Menu")]
    [SerializeField] private GameObject endingMenu;
    [SerializeField] private GameObject fadeOutPreviousCanvas;

    private readonly List<GameObject> _spawnedObjects = new List<GameObject>();

    void Start()
    {
        preOutcomeObject.SetActive(false);
        outcomeA.SetActive(false);
        outcomeB.SetActive(false);
        if (iconA != null) iconA.SetActive(false);
        if (iconB != null) iconB.SetActive(false);
        postOutcomeObject.SetActive(false);

        foreach (var obj in sequenceObjects)
            obj.SetActive(false);

        endingMenu.SetActive(false);

        StartCoroutine(RunOutcomeSequence());
    }

    private IEnumerator RunOutcomeSequence()
    {
        bool isPathA = PathTracker.Instance != null && PathTracker.Instance.IsPathA;
        GameObject icon = isPathA ? iconA : iconB;

        yield return new WaitForSeconds(timeBetweenSpawns);
        preOutcomeObject.SetActive(true);
        if (icon != null) icon.SetActive(true);
        if (icon != null) StartCoroutine(FadeObject(icon, 0f, 1f, fadeDuration));
        yield return StartCoroutine(FadeObject(preOutcomeObject, 0f, 1f, fadeDuration));

        yield return new WaitForSeconds(timeBetweenSpawns);
        GameObject outcome = isPathA ? outcomeA : outcomeB;
        outcome.SetActive(true);
        yield return StartCoroutine(FadeObject(outcome, 0f, 1f, fadeDuration));

        yield return new WaitForSeconds(timeBetweenSpawns);
        postOutcomeObject.SetActive(true);
        yield return StartCoroutine(FadeObject(postOutcomeObject, 0f, 1f, fadeDuration));

        Button postBtn = postOutcomeObject.GetComponentInChildren<Button>();
        if (postBtn != null)
            postBtn.onClick.AddListener(OnPostOutcomeButtonPressed);
    }

    private void OnPostOutcomeButtonPressed()
    {
        StartCoroutine(FadeOutOutcomesThenSpawn());
    }

    private IEnumerator FadeOutOutcomesThenSpawn()
    {
        StartCoroutine(FadeObject(preOutcomeObject, 1f, 0f, fadeDuration));

        bool isPathA = PathTracker.Instance != null && PathTracker.Instance.IsPathA;
        GameObject outcome = isPathA ? outcomeA : outcomeB;
        GameObject icon = isPathA ? iconA : iconB;
        StartCoroutine(FadeObject(outcome, 1f, 0f, fadeDuration));
        if (icon != null) StartCoroutine(FadeObject(icon, 1f, 0f, fadeDuration));
        StartCoroutine(FadeObject(postOutcomeObject, 1f, 0f, fadeDuration));

        yield return new WaitForSeconds(fadeDuration);

        preOutcomeObject.SetActive(false);
        outcome.SetActive(false);
        if (icon != null) icon.SetActive(false);
        postOutcomeObject.SetActive(false);

        yield return StartCoroutine(RunSpawnSequence());
    }

    private IEnumerator RunSpawnSequence()
    {
        for (int i = 0; i < sequenceObjects.Length; i++)
        {
            GameObject obj = sequenceObjects[i];
            obj.SetActive(true);
            _spawnedObjects.Add(obj);
            yield return StartCoroutine(FadeObject(obj, 0f, 1f, fadeDuration));

            bool isLast = i == sequenceObjects.Length - 1;
            if (!isLast)
                yield return new WaitForSeconds(timeBetweenSpawns);
        }

        if (sequenceObjects.Length > 0)
        {
            Button btn = sequenceObjects[^1].GetComponentInChildren<Button>();
            if (btn != null)
                btn.onClick.AddListener(OnFinalButtonPressed);
        }
    }

    private void OnFinalButtonPressed()
    {
        StartCoroutine(FadeInEndingMenu());
    }

    private IEnumerator FadeInEndingMenu()
    {
        endingMenu.SetActive(true);
        if (fadeOutPreviousCanvas != null)
        {
            StartCoroutine(FadeObject(fadeOutPreviousCanvas, 1f, 0f, fadeDuration));
            StartCoroutine(DeactivateAfterDelay(fadeOutPreviousCanvas, fadeDuration));
        }
        yield return StartCoroutine(FadeObject(endingMenu, 0f, 1f, fadeDuration));
    }

    private IEnumerator DeactivateAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }

    private IEnumerator FadeObject(GameObject obj, float from, float to, float duration)
    {
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();

        if (cg == null && sr == null && obj.GetComponent<RectTransform>() != null)
            cg = obj.AddComponent<CanvasGroup>();

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, elapsed / duration);
            ApplyAlpha(cg, sr, alpha);
            yield return null;
        }

        ApplyAlpha(cg, sr, to);
    }

    private static void ApplyAlpha(CanvasGroup cg, SpriteRenderer sr, float alpha)
    {
        if (cg != null)
        {
            cg.alpha = alpha;
        }
        else if (sr != null)
        {
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }
    }
}
