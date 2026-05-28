using UnityEngine;

public class DebugActivation : MonoBehaviour
{
    void OnEnable()
    {
        Debug.Log("[DebugActivation] " + gameObject.name + " activated by:\n" + StackTraceUtility.ExtractStackTrace());
    }
}
