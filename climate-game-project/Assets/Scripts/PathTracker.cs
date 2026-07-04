using UnityEngine;

public class PathTracker : MonoBehaviour
{
    public static PathTracker Instance { get; private set; }

    [Header("Prefabs spawned when path A is active")]
    [SerializeField] private GameObject[] pathAPrefabs;

    [Header("Prefabs spawned when path B is active")]
    [SerializeField] private GameObject[] pathBPrefabs;

    [Header("Container where path prefabs spawn")]
    [SerializeField] private Transform pathContainer;

    private bool _pathAChosen = true;
    private int _decisionIndex = 0;

    public bool PathChosen { get; private set; } = true;
    public bool IsPathA => _pathAChosen;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ResetPath()
    {
        _pathAChosen = true;
        PathChosen = true;
        _decisionIndex = 0;
        pathContainer = null;
    }

    public void ChoosePath(bool chooseA)
    {
        _pathAChosen = chooseA;
        PathChosen = true;
        _decisionIndex = 0;
    }

    // called at the same time as desicions being made, see VisualTracker
    public void SpawnNext()
    {
        if (!PathChosen) return;

        GameObject[] prefabs = _pathAChosen ? pathAPrefabs : pathBPrefabs;
        if (prefabs == null || _decisionIndex >= prefabs.Length) return;

        if (pathContainer == null)
        {
            GameObject found = GameObject.Find("PathContainer");
            if (found != null) pathContainer = found.transform;
            else return;
        }

        Instantiate(prefabs[_decisionIndex], pathContainer);
        _decisionIndex++;
    }
}
