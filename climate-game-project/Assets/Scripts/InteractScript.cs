using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class InteractScript : MonoBehaviour
{
    public GameObject interactPrompt;
    public GameObject clue;

    [Tooltip("The X button that appears once the clue's completion criteria is met.")]
    public GameObject closeButton;

    [SerializeField] private GameObject blurImage;

    public string playerTag = "Player";

    [Header("Text Replace on Dismiss")]
    public TMP_Text targetText;
    public string newText;
    public float fadeDuration = 0.5f;

    private bool playerInRange = false;
    private bool clueIsOpen = false;
    private bool textAlreadyChanged = false;
    private MovementScript playerMovement;
    private ClueBase activeClue;

    void Start()
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
        if (clue != null)
            clue.SetActive(false);
        if (closeButton != null)
            closeButton.SetActive(false);
    }

    void Update()
    {
        if (clueIsOpen)
        {
            // Show the X button as soon as the clue reports it's done.
            if (activeClue != null && activeClue.isDone)
            {
                if (closeButton != null && !closeButton.activeSelf)
                    closeButton.SetActive(true);
            }
            return;
        }

        if (playerInRange && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            Interact();
    }

    void Interact()
    {
        if (clue == null) return;

        clue.SetActive(true);
        activeClue = clue.GetComponent<ClueBase>();
        if (activeClue != null)
            activeClue.OnClueOpen();

        if (closeButton != null)
            closeButton.SetActive(false);

        if (blurImage != null)
            blurImage.SetActive(true);

        if (playerMovement != null)
            playerMovement.enabled = false;

        clueIsOpen = true;
    }

    // Hook this to the X button's OnClick event in the Inspector.
    public void DismissClue()
    {
        if (clue != null)
            clue.SetActive(false);
        if (closeButton != null)
            closeButton.SetActive(false);
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
        if (blurImage != null)
            blurImage.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = true;

        if (!textAlreadyChanged && targetText != null && !string.IsNullOrEmpty(newText))
        {
            textAlreadyChanged = true;
            StartCoroutine(FadeTextSwap());
        }

        clueIsOpen = false;
        playerInRange = false;
        activeClue = null;
    }

    IEnumerator FadeTextSwap()
    {
        Color c = targetText.color;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            c.a = 1f - t / fadeDuration;
            targetText.color = c;
            yield return null;
        }

        c.a = 0f;
        targetText.color = c;
        targetText.text = newText;

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
