using UnityEngine;

public interface RecipeSpawner
{
    void SpawnRecipe(CraftingRecipe recipe, uint numOfSpawnes = 1);
}


public class CraftingManager : MonoBehaviour
{

    private InventoryManager inventoryManager;
    private MinigameManager minigameManager;
    private RecipeSpawner recipeSpawner;
    private CraftingUI craftingUI;
    private PlayerUIManager uiManager;


    protected void Start()
    {
        recipeSpawner = null;
    }

    public void SetPlayer(Player player)
    {
        inventoryManager = player.Inventory;
        craftingUI = player.PlayerUI.CraftingUI;
        minigameManager = player.MinigameManager;
        uiManager = player.PlayerUI;
    }

    public void CraftRecipe(CraftingRecipe recipe, uint numToCraft = 1)
    {
        if (!inventoryManager.CanCraft(recipe, numToCraft)) return;
        //if (recipeSpawner == null) return; //<- add a check later 
        
        inventoryManager.RemoveIngredients(recipe, numToCraft);

        HandleCrafting(recipe, numToCraft);
        
        craftingUI.RefreshDescription();
    }

    protected virtual void HandleCrafting(CraftingRecipe recipe, uint numToCraft = 1)
    {

        if (recipe.buildingRequired == BuildingType.None)
        {
            Debug.Log("not yet");
            recipeSpawner.SpawnRecipe(recipe, numToCraft);
        }
        else
        {
            //close CraftingUI
            uiManager.ToggleCrafting();

            //start minigame 
            MinigameType minigameType = GetTypeFromBuilding(recipe.buildingRequired);
            minigameManager.StartMinigame(minigameType, numToCraft * recipe.craftingResult.quantity);
            minigameManager.SetOnFinishMinigame(recipe);
        }
    }

    public void SetRecipeSpawner(RecipeSpawner recipeSpawner = null)
    {
        this.recipeSpawner = recipeSpawner;
    }

    private MinigameType GetTypeFromBuilding(BuildingType buildingType)
    {
        switch (buildingType)
        {
            case BuildingType.Vinery:
                return MinigameType.Stomping;
            case BuildingType.Pot:
                return MinigameType.Mixing;
        }
        return MinigameType.None;
    }
}
