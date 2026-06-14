using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CheckboxClueAnimated : ClueBase
{
    [Header("Buttons")]
    public Button buttonA;
    public Button buttonB;

    [Header("Checkmarks")]
    public GameObject checkmarkA;
    public GameObject checkmarkB;

    [Header("Animation")]
    public Animator animTarget;
    public string animTrigger = "Play";
    [Tooltip("Activated at the halfway point of the animation")]
    public GameObject midAnimObject;

    [Header("Timing")]
    [Tooltip("Seconds to wait after checkmark appears before playing animation")]
    public float preAnimDelay = 1f;
    [Tooltip("Seconds to wait after animation finishes before showing close button")]
    public float postAnimDelay = 2f;
    [Tooltip("Seconds after animation starts before midAnimObject activates")]
    public float midAnimDelay = 1f;

    void Awake()
    {
        buttonA.onClick.AddListener(() => OnButtonClicked(checkmarkA));
        buttonB.onClick.AddListener(() => OnButtonClicked(checkmarkB));
    }

    public override void OnClueOpen()
    {
        isDone = false;

        checkmarkA.SetActive(false);
        checkmarkB.SetActive(false);

        buttonA.interactable = true;
        buttonB.interactable = true;

        animTarget.enabled = false;
    }

    void OnButtonClicked(GameObject checkmark)
    {
        checkmark.SetActive(true);

        buttonA.interactable = false;
        buttonB.interactable = false;

        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        yield return new WaitForSeconds(preAnimDelay);

        animTarget.enabled = true;
        animTarget.SetTrigger(animTrigger);

        // Wait one frame for the animator to transition into the new state
        yield return null;

        float clipLength = animTarget.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(midAnimDelay);
        if (midAnimObject != null)
            midAnimObject.SetActive(true);

        yield return new WaitForSeconds(clipLength - midAnimDelay);

        yield return new WaitForSeconds(postAnimDelay);

        isDone = true;
    }
}
