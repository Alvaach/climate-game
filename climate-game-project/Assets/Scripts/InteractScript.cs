using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class InteractScript : MonoBehaviour
{
    public GameObject interactPrompt;
    public GameObject clue;

    public string playerTag = "Player";

    [Header("Text Replace on Dismiss")]
    public TMP_Text targetText;
    public string newText;
    public float fadeDuration = 0.5f;

    private bool playerInRange = false;
    private bool waitingForClick = false;
    private bool textAlreadyChanged = false;
    private MovementScript playerMovement;

    void Start()
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
        if (clue != null)
            clue.SetActive(false);
    }

    void Update()
    {
        if (waitingForClick)
        {
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
                DismissClue();
            return;
        }

        if (playerInRange && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            Interact();
    }

    void Interact()
    {
        if (clue != null)
            clue.SetActive(true);

        if (playerMovement != null)
            playerMovement.enabled = false;

        waitingForClick = true;
    }

    void DismissClue()
    {
        if (clue != null)
            clue.SetActive(false);
        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = true;

        if (!textAlreadyChanged && targetText != null && !string.IsNullOrEmpty(newText))
        {
            textAlreadyChanged = true;
            StartCoroutine(FadeTextSwap());
        }

        waitingForClick = false;
        playerInRange = false;
    }

    IEnumerator FadeTextSwap()
    {
        // Fade out old text
        Color c = targetText.color;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            c.a = 1f - t / fadeDuration;
            targetText.color = c;
            yield return null;
        }

        // Swap text while invisible
        c.a = 0f;
        targetText.color = c;
        targetText.text = newText;

        // Fade in new text
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            c.a = t / fadeDuration;
            targetText.color = c;
            yield return null;
        }

        c.a = 1f;
        targetText.color = c;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = true;
            playerMovement = other.GetComponent<MovementScript>();
            if (interactPrompt != null)
                interactPrompt.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = false;
            if (interactPrompt != null)
                interactPrompt.SetActive(false);
        }
    }
}
