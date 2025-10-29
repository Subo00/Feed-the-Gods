using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : ThirdPersonMovement
{
    //Managers - getters
    public InventoryManager Inventory => inventory;
    public PlayerUIManager PlayerUI => playerUIManager;
    public MinigameManager MinigameManager => minigameManager;
    public DialogManager DialogManager => dialogManager;
    public CraftingManager CraftingManager => craftingManager;

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
    private Action interactAction;
    private Action delayedAction; 
    private Action cancleAction;
    private Action<Vector2> moveAction;
    private bool isInteracting = false;

    //Interacting - functions
    public bool IsInteracting => isInteracting;
    public void SetInteract(Action<Player> action) { interactActionPlayer = action;  }
    public void SetInteract(Action action) { interactAction = action;  }
    public void SetDelayedInteract(Action action) {  delayedAction = action; }
    public void SetCancle(Action action) { cancleAction = action; }
    public void SetOnMove(Action<Vector2> action) { moveAction = action; }
    public void ClearInteract()
    {
        interactActionPlayer = null;
        interactAction = null;
        delayedAction = null;
        cancleAction = null;
        moveAction = null;
    }

    /*
    private PlayerInputActions playerControls;
    private InputAction move;
    private InputAction interact;


    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }
    */
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


        ClearInteract();
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        /*if (Input.GetButtonDown("Fire1") && inventory.equippedItemStack != null && !isUIActive)
        {
            
            animationController.Play(Animations.THROW, 1, true, false, 0.16f);
            animationController.Play(Animations.NONE, 2, false, false, 0.16f);
        }*/
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
        uint itemToThrow = inventory.equippedItemStack.getItemData().id;
        DropItemFromPlayerPos(itemToThrow);
        inventory.RemoveFromEquippedStack();
    }

    public void OnMove(InputAction.CallbackContext context) 
    {
        if (moveAction != null)
        {
            moveAction(context.ReadValue<Vector2>());
            return;
        }
        SetInputVector(context.ReadValue<Vector2>());  
    }

    public void OnInteract(InputAction.CallbackContext context) 
    {
        if (context.started)
        {
            isInteracting = true;

            //if (!context.started) return;
            if (interactActionPlayer != null)
            {
                interactActionPlayer.Invoke(this);
            }
            else if (interactAction != null)
            {
                interactAction.Invoke();
            }

        }
        else if (context.canceled)
        {
            isInteracting = false;
            delayedAction?.Invoke();
        }
    }

    public void OnCancle(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if(cancleAction != null)
        {
            cancleAction.Invoke();
        }
        else
        {
            playerUIManager.CraftingUI.CancleCrafting();
        }
    }
}
