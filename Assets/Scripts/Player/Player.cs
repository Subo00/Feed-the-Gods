using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : ThirdPersonMovement
{
    public Inventory Inventory => inventory;
    public PlayerUIManager PlayerUI => playerUIManager;
    public MinigameManager MinigameManager => minigameManager;
    public bool IsInteracting => isInteracting;

    [SerializeField] private Transform playerSpawnItemPoint;
    [SerializeField] private Inventory inventory;
    [SerializeField] private MinigameManager minigameManager;
    [SerializeField] private PlayerUIManager playerUIManager;

    private ItemManager itemManager;
    private Action<Player> interactAction;
    private bool isInteracting = false;

    public void SetInteract(Action<Player> action) { interactAction = action;  }
    public void ClearInteract() {  interactAction = null; }
    
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
    protected override void Start()
    {
        //itemManager = ItemManager.Instance;

        //playerUIManager = CameraManager.Instance.CreatePlayerUI(this.transform);
        //playerUIManager.HideInteraction();

        //playerUIManager.InventoryUI.Bind(this);

        //PlayerInput playerInput = GetComponent<PlayerInput>();
        //playerInput.camera = playerUIManager.GetComponentInParent<Camera>();
        playerUIManager.SetPlayer(this);
        inventory.SetInevntoryUI(PlayerUI.InventoryUI);
        inventory.SetEquippedItemUI(PlayerUI.EquippedItemUI);

        minigameManager.SetPlayer(this);
        ClearInteract();
        base.Start();
    }

    protected override void Update()
    {
        //SetInputVector(move.ReadValue<Vector2>());
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

    public void OnMove(InputAction.CallbackContext context) { SetInputVector(context.ReadValue<Vector2>());  }

    public void OnInteract(InputAction.CallbackContext context) 
    {
        if (context.started) isInteracting = true;
        if (context.canceled) isInteracting = false;
        interactAction?.Invoke(this); 
    }
}
