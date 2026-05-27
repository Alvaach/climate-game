using System.Collections;
using UnityEngine;
using TMPro;

public class DragAndDropClue : ClueBase
{
    [Header("Text")]
    public TMP_Text displayText;
    public string initialText;
    public string resultA;
    public string resultB;
    public DraggableClueObject draggableObject;

    [Header("Drop Zones")]
    public RectTransform dropZoneA;
    public RectTransform dropZoneB;

    public float closeButtonDelay = 2f;

    public override void OnClueOpen()
    {
        isDone = false;
        displayText.text = initialText;
        draggableObject.Initialize(this);
    }

    public void OnDroppedOnA()
    {
        displayText.text = resultA;
        StartCoroutine(ShowCloseButtonAfterDelay());
    }

    public void OnDroppedOnB()
    {
        displayText.text = resultB;
        StartCoroutine(ShowCloseButtonAfterDelay());
    }

    IEnumerator ShowCloseButtonAfterDelay()
    {
        yield return new WaitForSeconds(closeButtonDelay);
        isDone = true;
    }
}
