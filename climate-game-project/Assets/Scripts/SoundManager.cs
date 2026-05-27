using UnityEngine;
using UnityEngine.InputSystem;

public class SoundManager : MonoBehaviour
{

    //why isnt hover sound removed from inspector when its removed here?
    public static SoundManager Instance { get; private set; }

    [Header("Add audio clips here")]
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private float clickVolume = 1f;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null) { Destroy(this); return; }
        Instance = this;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
            audioSource.PlayOneShot(clickSound, clickVolume);
    }
}
