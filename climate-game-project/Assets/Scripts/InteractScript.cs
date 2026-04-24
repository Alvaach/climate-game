using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class InteractScript : MonoBehaviour
{
    public GameObject interactPrompt;
    public GameObject clue;

    [Tooltip("Shared X button. This script registers/unregisters itself so only the active clue responds.")]
    public Button closeButton;

    [SerializeField] private GameObject blurImage;
    public GameObject continueObject;

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
            closeButton.gameObject.SetActive(false);
    }

    void Update()
    {
        if (clueIsOpen)
        {
            if (activeClue != null && activeClue.isDone)
            {
                if (closeButton != null && !closeButton.gameObject.activeSelf)
                {
                    closeButton.onClick.RemoveAllListeners();
                    closeButton.onClick.AddListener(DismissClue);
                    closeButton.gameObject.SetActive(true);
                }
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
            closeButton.gameObject.SetActive(false);

        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        if (blurImage != null)
            blurImage.SetActive(true);

        if (continueObject != null)
            continueObject.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = false;

        clueIsOpen = true;
    }

    public void DismissClue()
    {
        if (clue != null)
            clue.SetActive(false);
        if (closeButton != null)
        {
            closeButton.onClick.RemoveListener(DismissClue);
            closeButton.gameObject.SetActive(false);
        }
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
        if (blurImage != null)
            blurImage.SetActive(false);

        if (continueObject != null)
            continueObject.SetActive(true);

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
