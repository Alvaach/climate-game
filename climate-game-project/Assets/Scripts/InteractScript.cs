using UnityEngine;
using UnityEngine.InputSystem;

public class InteractScript : MonoBehaviour
{

    //need to add logic that halts the game until clue has been recieved!

    public GameObject interactPrompt;
    public GameObject clue;

    public string playerTag = "Player";

    private bool playerInRange = false;

    void Start()
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
        if (clue != null)
            clue.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Keyboard.current.eKey.isPressed)
            Interact();
    } 

    void Interact()
    {
        if (clue != null)
            clue.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = true;
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
