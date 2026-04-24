using UnityEngine;
using UnityEngine.InputSystem;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClip clickSound;
    [SerializeField] private float clickVolume = 1f;

    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private float hoverVolume = 1f;

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

    public void PlayHoverSound()
    {
        if (hoverSound != null && !audioSource.isPlaying)
            audioSource.PlayOneShot(hoverSound, hoverVolume);
    }
}
