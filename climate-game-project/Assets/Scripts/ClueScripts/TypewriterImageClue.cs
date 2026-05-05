using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterImageClue : ClueBase
{
    [TextArea(3, 10)] public string clueText;

    [Header("UI")]
    public TMP_Text textDisplay;

    [Header("Speed, letters per second")]
    public float typingSpeed = 30f;

    private Coroutine activeRoutine;

    public override void OnClueOpen()
    {
        isDone = false;

        if (textDisplay != null)
            textDisplay.text = "";

        if (activeRoutine != null)
            StopCoroutine(activeRoutine);

        activeRoutine = StartCoroutine(PlayClue());
    }

    IEnumerator PlayClue()
    {
        if (textDisplay != null && !string.IsNullOrEmpty(clueText))
        {
            textDisplay.text = "";
            float delay = typingSpeed > 0f ? 1f / typingSpeed : 0f;

            for (int i = 0; i < clueText.Length; i++)
            {
                textDisplay.text += clueText[i];
                yield return new WaitForSeconds(delay);
            }
        }

        isDone = true;
        activeRoutine = null;
    }
}
