using System.Collections;
using UnityEngine;

public class AnimatedRevealClue : ClueBase
{
    [Header("Canvas Image")]
    public GameObject clueImage;

    [Header("Animated Object")]
    public GameObject animatedObject;
    public string animationTrigger = "Play";

    [Header("Reveal Object")]
    public GameObject revealObject;
    public float revealDelay = 2f;

    private Coroutine activeRoutine;

    public override void OnClueOpen()
    {
        isDone = false;
        clueImage.SetActive(true);
        revealObject.SetActive(false);
        animatedObject.SetActive(true);

        if (activeRoutine != null)
            StopCoroutine(activeRoutine);

        activeRoutine = StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        Animator anim = animatedObject.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger(animationTrigger);

            yield return null; // let animator tick before sampling

            AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(state.length);
        }

        revealObject.SetActive(true);
        yield return new WaitForSeconds(revealDelay);

        isDone = true;
        activeRoutine = null;
    }

    void OnDisable()
    {
        if (activeRoutine != null)
        {
            StopCoroutine(activeRoutine);
            activeRoutine = null;
        }

        if (clueImage != null)
            clueImage.SetActive(false);

        if (animatedObject != null)
            animatedObject.SetActive(false);

        if (revealObject != null)
            revealObject.SetActive(false);
    }
}
