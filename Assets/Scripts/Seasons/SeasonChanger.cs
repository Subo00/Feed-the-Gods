using UnityEngine;

public class SeasonChanger : Interactable
{
    public override void OnInteract(Player player)
    {
        SeasonManager.Instance.ChangeSeason();
    }

    protected override void OnUpdate()
    {
        CommonLogic();
    }
}