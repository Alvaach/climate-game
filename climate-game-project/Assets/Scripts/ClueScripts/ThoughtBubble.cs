using System.Collections;
using UnityEngine;
using TMPro;

public class ThoughtBubble : ClueBase
{
    [Header("Animation")]
    [Tooltip("Can be a child of the player so it always appears at the same offset from them.")]
    public GameObject animatedObject;
    public Animator animator;

    [Header("Text")]
    public TMP_Text hintText;
    public float textFadeDuration = 0.5f;

    [Header("Timing")]
    [Tooltip("Seconds to wait after text fades in before the close button appears.")]
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
        if (animator != null)
        {
            yield return null; // let the Animator tick once before sampling state
            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f &&
                !animator.IsInTransition(0));
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
