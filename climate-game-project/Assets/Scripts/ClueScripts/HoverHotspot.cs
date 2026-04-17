using UnityEngine;
using UnityEngine.EventSystems;

// Attach to each hotspot area (a child Image/RectTransform on the clue).
// Shows a tooltip on hover and marks itself as visited.
public class HoverHotspot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Tooltip("The text or image that appears when the player hovers over this spot.")]
    public GameObject tooltip;

    [Tooltip("If true, this hotspot must be hovered for the clue to be completable.")]
    public bool required = true;

    [HideInInspector]
    public bool hasBeenHovered = false;

    private HoverTooltipClue parentClue;

    void Awake()
    {
        parentClue = GetComponentInParent<HoverTooltipClue>();

        if (tooltip != null)
            tooltip.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltip != null)
            tooltip.SetActive(true);

        if (!hasBeenHovered)
        {
            hasBeenHovered = true;
            parentClue?.CheckCompletion();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip != null)
            tooltip.SetActive(false);
    }

    public void Reset()
    {
        hasBeenHovered = false;
        if (tooltip != null)
            tooltip.SetActive(false);
    }
}
