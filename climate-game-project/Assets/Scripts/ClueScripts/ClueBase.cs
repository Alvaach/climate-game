using UnityEngine;

// Every clue script inherits from thiss
public abstract class ClueBase : MonoBehaviour
{
    public event System.Action<ClueBase> OnClueCompleted;

    private bool _isDone;

    [HideInInspector]
    public bool isDone
    {
        get => _isDone;
        set
        {
            if (value && !_isDone)
                OnClueCompleted?.Invoke(this);
            _isDone = value;
        }
    }

    // Called by InteractScript when the clue is opened
    public virtual void OnClueOpen() { }
}
