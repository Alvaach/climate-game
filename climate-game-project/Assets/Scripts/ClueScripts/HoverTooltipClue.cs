using UnityEngine;

public class HoverTooltipClue : ClueBase
{
    // Attach to the clue GameObject (the image that spawns when the player interacts).
    private HoverHotspot[] hotspots;

    void Awake()
    {
        hotspots = GetComponentsInChildren<HoverHotspot>(includeInactive: true);
    }

    public override void OnClueOpen()
    {
        isDone = false;
        foreach (var hotspot in hotspots)
            hotspot.Reset();
    }

    // Called by each HoverHotspot when it gets hovered for the first time.
    public void CheckCompletion()
    {
        foreach (var hotspot in hotspots)
        {
            if (hotspot.required && !hotspot.hasBeenHovered)
                return;
        }

        isDone = true;
    }

    // Completes when all required hotspots have been hovered.
}
