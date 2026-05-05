using UnityEngine;
using UnityEngine.UI;

public class CheckboxClue : ClueBase
{
    [Header("Buttons")]
    public Button buttonA;
    public Button buttonB;

    [Header("Checkmarks")]
    public GameObject checkmarkA;
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
