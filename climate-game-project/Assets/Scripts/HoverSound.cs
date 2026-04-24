using UnityEngine;
using UnityEngine.EventSystems;

// Attach to any GameObject to play the hover sound when the mouse enters it.
// UI elements work automatically. For 3D world objects, add a PhysicsRaycaster
// to the camera so the EventSystem can detect them.
public class HoverSound : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance?.PlayHoverSound();
    }

    // Fallback for 3D objects using Unity's built-in collision raycasting
    // (no EventSystem raycaster required).
    void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
            SoundManager.Instance?.PlayHoverSound();
    }
}
