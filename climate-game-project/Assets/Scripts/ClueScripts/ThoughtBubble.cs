using System.Collections;
using UnityEngine;
using TMPro;

public class ThoughtBubble : ClueBase
{
    [Header("Animation")]
    [Tooltip("Child that starts inactive. Activated when the hint opens.")]
    public GameObject animatedObject;

    [Header("Text")]
    public TMP_Text hintText;
    public float textFadeDuration = 0.5f;

    [Header("Timing")]
    [Tooltip("Seconds after text fades in before the close button appears.")]
    public float closeButtonDelay = 2f;

    public override void OnClueOpen()
    {
        isDone = false;

        if (hintText != null)
        {
            Color c = hintText.color;
            c.a = 0f;
            hintText.color = c;
        }

        if (animatedObject != null)
            animatedObject.SetActive(true);

        StartCoroutine(RunSequence());
    }

    IEnumerator RunSequence()
    {
        if (animatedObject != null)
        {
            Animator anim = animatedObject.GetComponent<Animator>();
            if (anim != null)
            {
                yield return null; // let animator tick before sampling
                yield return new WaitUntil(() =>
                    anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f &&
                    !anim.IsInTransition(0));
            }
        }

        if (hintText != null)
            yield return FadeText(0f, 1f);

        yield return new WaitForSeconds(closeButtonDelay);
        isDone = true;
    }

    void OnDisable()
    {
        if (animatedObject != null)
            animatedObject.SetActive(false);

        if (hintText != null)
        {
            Color c = hintText.color;
            c.a = 0f;
            hintText.color = c;
        }
    }

    IEnumerator FadeText(float from, float to)
    {
        Color c = hintText.color;
        for (float t = 0f; t < textFadeDuration; t += Time.deltaTime)
        {
            c.a = Mathf.Lerp(from, to, t / textFadeDuration);
            hintText.color = c;
            yield return null;
        }
        c.a = to;
        hintText.color = c;
    }
}
