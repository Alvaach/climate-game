using UnityEngine;

public class HoverTooltipClue : ClueBase
{
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

    // Called by HoverHotspot when it gets hovered for the first time.
    public void CheckCompletion()
    {
        foreach (var hotspot in hotspots)
        {
            if (hotspot.required && !hotspot.hasBeenHovered)
                return;
        }

        isDone = true;
    }

    // completes when all required hotspots have been touched/hovered?
}
