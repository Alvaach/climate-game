using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Add to each scene. Activates targetObject once every clue in the scene is completed.
public class ClueTracker : MonoBehaviour
{
    [Tooltip("GameObject to activate when all clues are done")]
    [SerializeField] private GameObject targetObject;

    private ClueBase[] clues;
    private readonly HashSet<ClueBase> completedClues = new HashSet<ClueBase>();

    void Start()
    {
        clues = FindObjectsOfType<ClueBase>(true);
        Debug.Log($"[ClueTracker] Found {clues.Length} clues: {string.Join(", ", System.Array.ConvertAll(clues, c => c.gameObject.name))}");

        foreach (var clue in clues)
            clue.OnClueCompleted += HandleClueCompleted;

        if (targetObject != null)
            targetObject.SetActive(false);
    }

    void OnDestroy()
    {
        if (clues == null) return;
        foreach (var clue in clues)
            if (clue != null)
                clue.OnClueCompleted -= HandleClueCompleted;
    }

    void HandleClueCompleted(ClueBase clue)
    {
        completedClues.Add(clue);
        Debug.Log($"[ClueTracker] Clue completed: {clue.gameObject.name} ({completedClues.Count}/{clues.Length})");
        if (completedClues.Count >= clues.Length && targetObject != null)
        {
            targetObject.SetActive(true);
            Animator anim = targetObject.GetComponent<Animator>();
            if (anim != null)
                StartCoroutine(PlayBouncyThenIdle(anim));
        }
    }

    IEnumerator PlayBouncyThenIdle(Animator anim)
    {
        anim.Play("Bouncy");
        yield return null;
        while (anim.GetCurrentAnimatorStateInfo(0).IsName("Bouncy") &&
               anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            yield return null;
        anim.Play("Continue, level1");
    }

    public int TotalClues => clues?.Length ?? 0;
    public int CompletedCount => completedClues.Count;
}
