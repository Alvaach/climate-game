using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class InteractScript : MonoBehaviour
{
    public GameObject interactPrompt;
    public GameObject clue;

    [Header("Shared X button. This script activates itself ish so only the active clue responds.")]
    public Button closeButton;

    [SerializeField] private GameObject blurImage;
    public GameObject continueObject;

    public string playerTag = "Player";

    [Header("???? to replace with clue")]
    public TMP_Text targetText;
    public string newText;
    public Animator textAnimator;
    public string revealAnimationName = "TextReveal";

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
            SwapTextWithAnimation();
        }

        clueIsOpen = false;
        playerInRange = false;
        activeClue = null;
    }

    void SwapTextWithAnimation()
    {
        Color c = targetText.color;
        c.a = 1f;
        targetText.color = c;
        targetText.text = newText;

        if (textAnimator != null && !string.IsNullOrEmpty(revealAnimationName))
        {
            textAnimator.Rebind();
            textAnimator.Play(revealAnimationName, 0, 0f);
        }
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
