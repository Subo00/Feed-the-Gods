using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManagerChild : CraftingManager
{
    private MinigameManager minigameManager;

    private void Start()
    {
        minigameManager = MinigameManager.Instance;
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
            UIManagerChild.InstanceChild.ToggleCrafting();

            //start minigame 
            MinigameManager.MinigameType minigameType = GetTypeFromBuilding(recipe.buildingRequired);
            minigameManager.StartMinigame(minigameType, numToCraft);
            minigameManager.SetOnFinishMinigame(recipe);
        }
    }

    private MinigameManager.MinigameType GetTypeFromBuilding(BuildingType buildingType)
    {
        switch (buildingType)
        {
            case BuildingType.Vinery:
                return MinigameManager.MinigameType.Stomping;
        }
        return MinigameManager.MinigameType.None;
    }
}
