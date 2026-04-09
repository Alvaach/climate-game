using UnityEngine;

public class VisualTracker : MonoBehaviour
{
    public static VisualTracker Instance { get; private set; }

    [Header("Box Prefabs")]
    [SerializeField] private GameObject greenBoxPrefab;
    [SerializeField] private GameObject redBoxPrefab;

    [Header("Container where boxes spawn")]
    [SerializeField] private Transform boxContainer;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
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

        GameObject prefab = amount > 0 ? greenBoxPrefab : redBoxPrefab;
        int count = Mathf.Abs(amount);

        for (int i = 0; i < count; i++)
            Instantiate(prefab, boxContainer);
    }
}
