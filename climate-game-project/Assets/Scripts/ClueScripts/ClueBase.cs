using UnityEngine;

// Every clue script inherits from thiss
public abstract class ClueBase : MonoBehaviour
{
    [HideInInspector]
    public bool isDone = false;

    // Called by InteractScript when the clue is opened
    public virtual void OnClueOpen() { }
}
