using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public Image blackScreen;

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    public void FadeAndLoad(string sceneName, float duration)
    {
        StartCoroutine(Fader(sceneName, duration));
    }

    IEnumerator Fader(string sceneName, float duration)
    {
        float t = 0;
        Color c = blackScreen.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = t / duration;
            blackScreen.color = c;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeOut()
    {
        float t = 0;
        Color c = blackScreen.color;

        while (t < 1)
        {
            t += Time.deltaTime;
            c.a = 1f - (t / 1f);
            blackScreen.color = c;
            yield return null;
        }
    }
}