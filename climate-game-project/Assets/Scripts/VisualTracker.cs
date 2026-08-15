using UnityEngine;

public class VisualTracker : MonoBehaviour
{
    public static VisualTracker Instance { get; private set; }

    [Header("Boxes that will spawn")]
    [SerializeField] private GameObject greenBoxPrefab;
    [SerializeField] private GameObject redBoxPrefab;

    [Header("Place where boxes spawn")]
    [SerializeField] private Transform boxContainer;

    private static Transform cachedBoxContainer;

    void Awake()
    {
        Instance = this;
    }

    public static void ResetBoxes()
    {
        if (cachedBoxContainer == null) return;

        foreach (Transform child in cachedBoxContainer)
            Destroy(child.gameObject);
    }

    public void AddVisual(int amount)
    {
        if (amount == 0) return;

        if (boxContainer == null)
        {
            GameObject found = GameObject.Find("BoxContainer");
            if (found != null) boxContainer = found.transform;
            else return;
        }

        cachedBoxContainer = boxContainer;

        GameObject prefab = amount > 0 ? greenBoxPrefab : redBoxPrefab;
        int count = Mathf.Abs(amount);

        for (int i = 0; i < count; i++)
            Instantiate(prefab, boxContainer);
    }
}
