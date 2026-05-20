using UnityEngine;

[RequireComponent(typeof(ClueBase))]
public class AutoStartClue : MonoBehaviour
{
    void OnEnable()
    {
        GetComponent<ClueBase>().OnClueOpen();
    }
}
