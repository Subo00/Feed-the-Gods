
using UnityEngine;

public class Source : SourceBase
{
    [SerializeField] private MinigameManager.MinigameType minigameType;

    protected override void OnInteracted()
    {
        MinigameManager.Instance.SetOnFinishMinigame(DropResource);
        MinigameManager.Instance.StartMinigame(minigameType);
        inUse = true;
    }
}
