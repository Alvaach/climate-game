using UnityEngine;
using UnityEngine.EventSystems;

public class HoverSound : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance?.PlayHoverSound();
    }

    void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
            SoundManager.Instance?.PlayHoverSound();
    }
}
