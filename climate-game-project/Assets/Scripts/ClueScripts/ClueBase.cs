using UnityEngine;

// Every clue script inherits from this.
// InteractScript watches isDone to know when to show the X button.
public abstract class ClueBase : MonoBehaviour
{
    [HideInInspector]
    public bool isDone = false;

    // Called by InteractScript when the clue is opened, so each clue can reset itself.
    public virtual void OnClueOpen() { }
}
