
using UnityEngine;

public class Source : SourceBase
{
    [SerializeField] private MinigameType minigameType;

    protected override void OnInteracted()
    {
        //base.OnInteracted();
        interactingPlayer.MinigameManager.SetOnFinishMinigame(DropResource);
        interactingPlayer.MinigameManager.SetOnFailMinigame(() => inUse = false);
        interactingPlayer.MinigameManager.StartMinigame(minigameType);
        inUse = true;
    }
}
