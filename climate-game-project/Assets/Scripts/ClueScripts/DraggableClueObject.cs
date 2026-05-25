using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableClueObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private DragAndDropClue clue;
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 startPosition;
    private bool dropped;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        startPosition = rectTransform.anchoredPosition;
    }

    public void Initialize(DragAndDropClue parentClue)
    {
        clue = parentClue;
        rectTransform.anchoredPosition = startPosition;
        dropped = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (dropped) return;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dropped) return;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dropped) return;

        if (Overlaps(rectTransform, clue.dropZoneA))
        {
            SnapTo(clue.dropZoneA);
            dropped = true;
            clue.OnDroppedOnA();
        }
        else if (Overlaps(rectTransform, clue.dropZoneB))
        {
            SnapTo(clue.dropZoneB);
            dropped = true;
            clue.OnDroppedOnB();
        }
        else
        {
            rectTransform.anchoredPosition = startPosition;
        }
    }

    void SnapTo(RectTransform zone)
    {
        rectTransform.position = zone.position;
    }

    static bool Overlaps(RectTransform a, RectTransform b)
    {
        return WorldRect(a).Overlaps(WorldRect(b));
    }

    static Rect WorldRect(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        return new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);
    }
}
