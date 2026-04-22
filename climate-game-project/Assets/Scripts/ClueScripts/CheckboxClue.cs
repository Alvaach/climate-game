using UnityEngine;
using UnityEngine.UI;

// Attach to the clue root GameObject.
// Two buttons — clicking either shows a checkmark on that button,
// disables both buttons, and sets isDone = true so the close button appears.
public class CheckboxClue : ClueBase
{
    [Header("Buttons")]
    public Button buttonA;
    public Button buttonB;

    [Header("Checkmarks")]
    [Tooltip("Checkmark child object inside Button A")]
    public GameObject checkmarkA;
    [Tooltip("Checkmark child object inside Button B")]
    public GameObject checkmarkB;

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
    }

    void OnButtonClicked(GameObject checkmark)
    {
        checkmark.SetActive(true);

        buttonA.interactable = false;
        buttonB.interactable = false;

        isDone = true;
    }
}
