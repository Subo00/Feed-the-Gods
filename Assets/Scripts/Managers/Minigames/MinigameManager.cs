using System;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public Player Player => player;

    private Minigame currentMinigame;
    private GameObject currentMinigamePrefab = null;
    private MinigameUI minigameCanvas;
    private Player player;

    private System.Action<float> onFinishMinigameSource;
    private System.Action onFailMinigameSource;
    private CraftingRecipe recipe = null;
    private RecipeSpawner recipeSpawner = null;
    private bool? isSource = null;
    private Action onEndMinigame = null;


    public void SetPlayer(Player player) 
    { 
        this.player = player;
        minigameCanvas = player.PlayerUI.MinigameUI;
    }

    public void SetOnFinishMinigame(System.Action<float> action)
    {
        onFinishMinigameSource = action;
        isSource = true;
    }

    public void SetOnFailMinigame(System.Action action){ onFailMinigameSource = action; }
    public void SetOnEndAction(Action action) { onEndMinigame = action; }
    public void SetOnFinishMinigame(CraftingRecipe recipe)
    {
        this.recipe = recipe;
        isSource = false;
    }

    public void StartMinigame(MinigameType minigameType,  uint valueUint = 1)
    {
        //minigameCanvas.Toggle(true);
        switch (minigameType)
        {
            case MinigameType.Stomping:
                currentMinigamePrefab = Instantiate(MinigamePrefabHolder.Instance.GetStomping() , minigameCanvas.transformObject.transform);
                break;
            case MinigameType.Gathering:
                currentMinigamePrefab = Instantiate(MinigamePrefabHolder.Instance.GetGathering() , minigameCanvas.transformObject.transform);
                break;
            case MinigameType.HeadHit:
                currentMinigamePrefab = Instantiate(MinigamePrefabHolder.Instance.GetHeadHit() , minigameCanvas.transformObject.transform);
                break;
                // Add other minigame types here
        }

        if (currentMinigamePrefab != null)
        {
            currentMinigame = currentMinigamePrefab.GetComponent<Minigame>();
            //uiManagerChild.ToggleMinigame();
            player.PlayerUI.ToggleMinigame();

            currentMinigame.StartMinigame(valueUint, this);
        }
    }

    public void EndMinigame(float success)
    {
        currentMinigame.EndMinigame();
        player.PlayerUI.ToggleMinigame();
        onEndMinigame?.Invoke();
        
        currentMinigame = null;
        onEndMinigame = null;

        Destroy(currentMinigamePrefab);
        currentMinigamePrefab = null;

        //if success then drop reward 
        if(success > 0f)
        {
            if(isSource == true)
            {
                onFinishMinigameSource(success);
            }
            else
            {
                recipeSpawner.SpawnRecipe(recipe/*,(uint) success*/);
            }
        }
        else
        {
            if(onFailMinigameSource != null)
            {
                onFailMinigameSource();
            }
        }

        isSource = null;
        recipe = null;
        minigameCanvas.Toggle(false);
        player.ClearInteract();
        player.SetLastInteract();
    }

    public void SetRecipeSpawner(RecipeSpawner spawner)
    {
        recipeSpawner = spawner;
    }

    public void SpawnFromSpawner()
    {
        recipeSpawner.SpawnRecipe(recipe);
    }

}
