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
        if (completedClues.Count >= clues.Length && targetObject != null)
            targetObject.SetActive(true);
    }

    public int TotalClues => clues?.Length ?? 0;
    public int CompletedCount => completedClues.Count;
}
