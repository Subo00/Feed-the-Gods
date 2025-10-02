using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManagerChild : CraftingManager
{
    private MinigameManager minigameManager;

    private void Start()
    {
        base.Start();
    }

    protected override void HandleCrafting(CraftingRecipe recipe, uint numToCraft = 1)
    {
        if (recipe.buildingRequired == BuildingType.None)
        {
            Debug.Log("not yet");
            base.HandleCrafting(recipe, numToCraft);
        }
        else
        {
            //close CraftingUI
            UIManagerChild.Instance.ToggleCrafting();

            //start minigame 
            MinigameType minigameType = GetTypeFromBuilding(recipe.buildingRequired);
            //minigameManager.StartMinigame(minigameType, numToCraft);
            //minigameManager.SetOnFinishMinigame(recipe);
        }
    }

    private MinigameType GetTypeFromBuilding(BuildingType buildingType)
    {
        switch (buildingType)
        {
            case BuildingType.Vinery:
                return MinigameType.Stomping;
        }
        return MinigameType.None;
    }
}
