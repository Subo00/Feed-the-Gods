using SmallHedge.SoundManager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : ThirdPersonMovement, RecipeSpawner
{
    //Managers - getters
    public InventoryManager Inventory => inventory;
    public PlayerUIManager PlayerUI => playerUIManager;
    public MinigameManager MinigameManager => minigameManager;
    public DialogManager DialogManager => dialogManager;
    public CraftingManager CraftingManager => craftingManager;
    public PlayerAnimationController Animator => animationController;

    //Managers - variables
    [SerializeField] private Transform playerSpawnItemPoint;
    [SerializeField] private InventoryManager inventory;
    [SerializeField] private MinigameManager minigameManager;
    [SerializeField] private PlayerUIManager playerUIManager;
    [SerializeField] private DialogManager dialogManager;
    [SerializeField] private CraftingManager craftingManager;
    private ItemManager itemManager;

    //Interacting - variables
    private PlayerInput playerInput;
    private Action<Player> interactActionPlayer;
    private Action<Player> interactActionPlayerLast;
    private Action interactAction;
    private Action itemAction;
    private Action delayedAction; 
    private List<Action> cancleActions;
    private Action<Vector2> moveAction;
    private bool isInteracting = false;
    private bool controlEnabled = true;

    //Interacting - functions
    public bool IsInteracting => isInteracting;
    public void SetInteract(Action<Player> action) { interactActionPlayer = action; interactActionPlayerLast = action;  }
    public void SetInteract(Action action) { interactAction = action;  }
    public void SetItemAction(Action action) {  itemAction = action; }
    public void SetDelayedInteract(Action action) {  delayedAction = action; }
    public void AddOnCancle(Action action) { if(action != null) cancleActions.Add(action); }
    public void SetOnMove(Action<Vector2> action) { moveAction = action; }
    public void ClearInteract()
    {
        interactActionPlayer = null;
        interactAction = null;
        delayedAction = null;
        cancleActions.Clear();
        moveAction = null;
    }
    public void SetLastInteract() { interactActionPlayer = interactActionPlayerLast;   }
    public void SetControlEnable(bool enabled) 
    { 
        controlEnabled = enabled;
        playerUIManager.SetControlsEnable(enabled);
    }
   
    public override void ToggleUI(bool isActive)
    {
        isUIActive = isActive;
    }
    protected override void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        if(playerInput == null)
        {
            Debug.LogError("no player input on player");
        }

        playerUIManager.SetPlayer(this);
        inventory.SetInevntoryUI(PlayerUI.InventoryUI);
        inventory.SetEquippedItemUI(PlayerUI.EquippedItemUI);

        minigameManager.SetPlayer(this);
        dialogManager.SetPlayer(playerUIManager);
        craftingManager.SetPlayer(this);

        playerUIManager.CraftingUI.SetPlayer(this);

        itemManager = ItemManager.Instance;

        if (playerInput.currentControlScheme == "WASD")
        {
            string s = Keyboard.current.ToString();
            if(s.Contains("WASD")) //one player is WASD
            {
                playerUIManager.ChangePrompt(InputPrompt.WASD);
            }
            else //another is IJKL 
            {
                playerUIManager.ChangePrompt(InputPrompt.IJKL);
            }
        }
        else
        {
            playerUIManager.ChangePrompt(InputPrompt.XBOX);
        }

        cancleActions = new List<Action>();

        ClearInteract();
        base.Start();
    }

    protected override void Update()
    {
        if (!controlEnabled) return;
        base.Update();

    }

    public void DropItemFromPlayerPos(uint itemID)
    {
        //Dropt it from player's position 
        Vector3 playerPos = playerSpawnItemPoint.position + transform.forward * 1f; //magic number 3
        Quaternion playerRot = transform.rotation;

        //Add Instantiate 
        GameObject itemToDrop = itemManager.GetGameObject(itemID);
        //Vector3 spawnPoint = playerPos + playerSpawnItemPoint;
        GameObject drop = Instantiate(itemToDrop, playerPos, playerRot);
        float launchForce = direction.magnitude == 0 ? 5f : 9f;  //magic number 5
        drop.GetComponent<ItemPickUp>().LaunchInDirection(transform.forward + transform.up, launchForce);
    }

    public void Throw()
    {
        if (!controlEnabled) return;

        if (inventory.equippedItemStack == null) return;
        uint itemToThrow = inventory.equippedItemStack.getItemData().id;
        DropItemFromPlayerPos(itemToThrow);
        inventory.RemoveFromEquippedStack();
    }

    public void OnMove(InputAction.CallbackContext context) 
    {
        if (!controlEnabled) return;

        if (moveAction != null)
        {
            moveAction(context.ReadValue<Vector2>());
            return;
        }
        SetInputVector(context.ReadValue<Vector2>());  
    }

    public void OnInteract(InputAction.CallbackContext context) 
    {
        if (!controlEnabled) return;

        if (context.started)
        {
            isInteracting = true;

            //if (!context.started) return;
            if (interactActionPlayer != null)
                interactActionPlayer.Invoke(this);
            else if (interactAction != null)
                interactAction.Invoke();
        }
        else if (context.canceled)
        {
            isInteracting = false;
            delayedAction?.Invoke();
        }
    }

    public void OnCancle(InputAction.CallbackContext context)
    {
        if (!controlEnabled) return;

        if (!context.started) return;
        if(cancleActions.Count != 0)
        {
            foreach(var action in cancleActions)
                action.Invoke();
        }
        else
        {
            Throw();
        }
    }

    public void OnUseItem()
    {
        if (!controlEnabled) return;

        itemAction?.Invoke();
    }

    public void SpawnRecipe(CraftingRecipe recipe, uint numOfSpawnes)
    {
        uint itemID = recipe.craftingResult.item.id;
        uint quantity = recipe.craftingResult.quantity * numOfSpawnes;

        //Dropt it from object's position 
        Vector3 objectPos = transform.position + transform.forward * 3; //magic number 3
        Quaternion objectRot = transform.rotation;

        while (quantity > 0)
        {
            //TODO: CHANGE THIS A BIT 
            //Add Instantiate 
            GameObject itemToDrop = itemManager.GetGameObject(itemID);
            GameObject drop = Instantiate(itemToDrop, objectPos, objectRot);
            drop.GetComponent<ItemPickUp>().LaunchInDirection(transform.forward + transform.up, 5f); //magic number 5
            SoundManager.PlaySound(SoundType.ItemSpawned);

            quantity--;
        }
    }

}
