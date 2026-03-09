using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MinigameManager : MonoBehaviour
{
    public Player Player => player;

    //public bool isInteracting = true;

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

    //Actions for minigames
    private Action actionUp;
    private Action actionDown;
    private Action actionLeft;
    private Action actionRight;
    private Action actionInteract;
    private Action actionCancle;

    //SETTERS
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
    public void SetOnFailMinigame(System.Action action) { onFailMinigameSource = action; }
    public void SetOnEndAction(Action action) { onEndMinigame = action; }
    public void SetOnFinishMinigame(CraftingRecipe recipe)
    {
        this.recipe = recipe;
        isSource = false;
    }
    public void SetRecipeSpawner(RecipeSpawner spawner)
    {
        recipeSpawner = spawner;
    }

    //SETTERS FOR ACTIONS
    public void SetOnUp(Action action) { actionUp = action; }
    public void SetOnDown(Action action) { actionDown = action; }
    public void SetOnLeft(Action action) { actionLeft = action; }
    public void SetOnRight(Action action) { actionRight = action; }
    public void SetInteract(Action action) { actionInteract = action; }
    public void SetCancle(Action action) { actionCancle = action; }

    public void StartMinigame(MinigameType minigameType, uint valueUint = 1)
    {
        //minigameCanvas.Toggle(true);

        currentMinigamePrefab = Instantiate(MinigamePrefabHolder.Instance.GetMinigame(minigameType), minigameCanvas.transformObject.transform);

        if (currentMinigamePrefab != null)
        {
            currentMinigame = currentMinigamePrefab.GetComponent<Minigame>();
            player.PlayerUI.ToggleMinigame();
            //player.InputHandler.ChangeActionMap(ActionMap.Minigame);
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
        if (success > 0f)
        {
            if (isSource == true)
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
            if (onFailMinigameSource != null)
            {
                onFailMinigameSource();
            }
        }

        isSource = null;
        recipe = null;
        minigameCanvas.Toggle(false);
        ClearInteract();
        player.InputHandler.ChangeActionMap(ActionMap.Player);
        player.Animator.PlayAllLayers(Animations.IDLE);

        player.InputHandler.ClearInteract();
        player.SetLastInteract();
    }
    public void SpawnFromSpawner()
    {
        recipeSpawner.SpawnRecipe(recipe);
    }

        
    public void OnUp(InputAction.CallbackContext context){ if (context.started) actionUp?.Invoke();}
    public void OnDown(InputAction.CallbackContext context) { if(context.started) actionDown?.Invoke();}
    public void OnLeft(InputAction.CallbackContext context) { if(context.started) actionLeft?.Invoke();} 
    public void OnRight(InputAction.CallbackContext context) { if (context.started) actionRight?.Invoke(); }
    public void OnInteract(InputAction.CallbackContext context) { if (context.started) actionInteract?.Invoke();}
    public void OnCancle(InputAction.CallbackContext context) {  if(context.started){ actionCancle?.Invoke();  actionCancle = null; } }


    private void ClearInteract()
    {
        actionUp = null;
        actionDown = null;
        actionLeft = null;
        actionRight = null;
        actionInteract = null;
    }
}
