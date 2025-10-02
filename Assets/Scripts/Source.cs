
using UnityEngine;

public class Source : SourceBase
{
    [SerializeField] private MinigameType minigameType;

    protected override void OnInteracted()
    {
        //base.OnInteracted();
        interactingPlayer.MinigameManager.SetOnFinishMinigame(DropResource);
        interactingPlayer.MinigameManager.StartMinigame(minigameType);
        inUse = true;
    }
}
