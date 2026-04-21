using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioClip music;
    [SerializeField] private float maxVolume = 0.5f;
    [SerializeField] private float fadeDuration = 2f;

    private AudioSource audioSource;
    private Coroutine fadeLoopCoroutine;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = music;
        audioSource.loop = false;
        audioSource.volume = 0f;
        audioSource.Play();

        fadeLoopCoroutine = StartCoroutine(FadeLoopRoutine());
    }

    private IEnumerator FadeLoopRoutine()
    {
        yield return StartCoroutine(Fade(0f, maxVolume, fadeDuration));

        while (true)
        {
            float timeUntilFadeOut = audioSource.clip.length - audioSource.time - fadeDuration;
            if (timeUntilFadeOut > 0f)
                yield return new WaitForSeconds(timeUntilFadeOut);

            yield return StartCoroutine(Fade(maxVolume, 0f, fadeDuration));

            audioSource.Stop();
            audioSource.time = 0f;
            audioSource.Play();

            yield return StartCoroutine(Fade(0f, maxVolume, fadeDuration));
        }
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        audioSource.volume = to;
    }
}
