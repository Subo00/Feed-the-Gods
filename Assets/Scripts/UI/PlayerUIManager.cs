using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class PlayerUIManager : MonoBehaviour, UIPrompt
{
    public PlayerHUD PlayerHUD => playerHUD;
    public MinigameUI MinigameUI => minigameUI;
    public InventoryUI InventoryUI => inventoryUI;
    public EquippedItemUI EquippedItemUI => equippedItemUI;
    public CraftingUI CraftingUI => craftingUI; 
    public DialogUI DialogUI => dialogUI;
    public MenuUI MenuUI => menuUI;

    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private CraftingUI craftingUI;
    [SerializeField] private MinigameUI minigameUI;
    [SerializeField] private DialogUI dialogUI;
    [SerializeField] private PlayerHUD playerHUD;
    [SerializeField] private EquippedItemUI equippedItemUI;
    [SerializeField] private MenuUI menuUI;

    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Camera cam;

    private Player player;
    private bool isInventoryOpen = false;
    private bool isDialogOpen = false;
    private bool interactionActive = true; //used for DialogManager
    private bool isCraftingOpen = false;
    private bool isMinigameOpen = false;
    private bool isMenuOpen = false;

    private GameObject previousButton = null;

    private enum UIType { None, Inventory, Crafting, Minigame, Menu, Dialog, Tutorial };
    UIType currentType = UIType.None;
    UIType previousType = UIType.None;

    void Start()
    {
        craftingUI.Toggle(false);
        minigameUI.Toggle(false);
        dialogUI.Toggle(false);
        inventoryUI.Toggle(false);
        menuUI.Initialize();
        menuUI.SetResumeButton(ToggleMenu);
        menuUI.Toggle(false);

        cam = GetComponentInParent<Camera>();
    }

    public void SetPlayer(Player player) 
    { 
        this.player = player; 
        inventoryUI.Bind(player);
        craftingUI.Bind(player);
    }
    
    public void ToggleInventoryTrigger(InputAction.CallbackContext context)
    {
        if (context.performed) ToggleInventory();
    }

    public void SetControlsEnable(bool enabled) { eventSystem.enabled = enabled; }

    public void ToggleMenuTrigger(InputAction.CallbackContext context)
    {
        if (context.performed) ToggleMenu();
    }

    public void ToggleInventory()
    {
        if (IsImportatnTypeOpen())
            return;

        isInventoryOpen = !isInventoryOpen;
        inventoryUI.Toggle(isInventoryOpen);
        player.ToggleUI(isInventoryOpen);

        if (isInventoryOpen)
        {
            CloseOtherUIs(UIType.Inventory);
            inventoryUI.Toggle(isInventoryOpen);
            SetCurrentUIType(UIType.Inventory);
            eventSystem.SetSelectedGameObject(inventoryUI.GetFirstButton());
            player.AddOnCancle(ToggleInventory);
        }
        else
        {
            CloseOtherUIs(UIType.None);
        }
    }


    public void ToggleDialog()
    {
        isDialogOpen = !isDialogOpen;

        if (isDialogOpen)
        {
            CloseOtherUIs(UIType.Dialog);
            //SetUIPosition(UIType.Dialog);
            dialogUI.Toggle(isDialogOpen);
            SetCurrentUIType(UIType.Dialog);
            interactionActive = false;
        }
        else
        {
            CloseOtherUIs(UIType.None);
            interactionActive = true;
        }
    }

    private void SetCurrentUIType(UIType type)
    {
        previousType = currentType;
        currentType = type;
    }

    
    private void CloseOtherUIs(UIType currentUI)
    {
        if (currentUI != UIType.Inventory)
        {
            isInventoryOpen = false;
            inventoryUI.Toggle(isInventoryOpen);
        }
        else
        {
            equippedItemUI.Toggle(false);
        }

        if (currentUI != UIType.Dialog)
        {
            isDialogOpen = false;
            dialogUI.Toggle(isDialogOpen);
        }

        if(currentUI != UIType.Crafting)
        {
            isCraftingOpen = false;
            craftingUI.Toggle(isCraftingOpen);
        }

        if (currentUI != UIType.Minigame)
        {
            isMinigameOpen = false;
            minigameUI.Toggle(isMinigameOpen);
        }

        if (currentUI != UIType.Menu)
        {
            isMenuOpen = false;
            menuUI.Toggle(isMenuOpen);
        }

        if (currentUI == UIType.None)
        {
            player.ToggleUI(false);
            equippedItemUI.Toggle(true);
            SetCurrentUIType(UIType.None);
            eventSystem.SetSelectedGameObject(null);
        }
        else
        {
            player.ToggleUI(true);
            //equippedItemUI.Toggle(false);
        }
    }

    /*private void SetUIPosition(UIType uiType)
    {
        // Convert the world point to screen point
        Vector3 screenPoint = cam.WorldToScreenPoint(lastInteractPoint.position);

        // Check if the point is in front of the camera
        if (screenPoint.z > 0)
        {
            // Convert screen point to canvas space
            Vector2 canvasPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPoint, canvas.worldCamera, out canvasPos);

            switch (uiType)
            {
                case UIType.None:
                    return;
                case UIType.Minigame:
                    minigameUI.transformObject.GetComponent<RectTransform>().anchoredPosition = canvasPos;
                    return;
                case UIType.Dialog:
                    dialogUI.transformObject.GetComponent<RectTransform>().anchoredPosition = canvasPos;
                    return;
            }
        }

    }*/

    public void ShowDialogResponse(bool show)
    {
        eventSystem.SetSelectedGameObject(show ? dialogUI.GetFirstButton() : null);
        interactionActive = show;
    }

    public void ToggleCraftingNone()
    {
        if (IsImportatnTypeOpen())
            return;
        craftingUI.UpdateRecipeList(BuildingType.None);
        player.CraftingManager.SetRecipeSpawner(player);
        ToggleCrafting();
    }

    private bool IsImportatnTypeOpen()
    {
        switch (currentType)
        {
            case UIType.Menu:
            case UIType.Minigame:
            case UIType.Dialog:
                return true;
            default:
                return false;
        }
    }

    public void ToggleCrafting()
    {
        isCraftingOpen = !isCraftingOpen;


        if (isCraftingOpen)
        {
            CloseOtherUIs(UIType.Crafting);
            craftingUI.Toggle(isCraftingOpen);
            SetCurrentUIType(UIType.Crafting);
            eventSystem.SetSelectedGameObject(craftingUI.GetFirstButton());
            player.AddOnCancle(ToggleCrafting);
        }
        else
        {
            CloseOtherUIs(UIType.None);
        }
    }

    public void ToggleMinigame()
    {
        isMinigameOpen = !isMinigameOpen;

        if (isMinigameOpen)
        {
            CloseOtherUIs(UIType.Minigame);


            minigameUI.Toggle(isMinigameOpen);
            SetCurrentUIType(UIType.Minigame);
        }
        else
        {
            CloseOtherUIs(UIType.None);
        }
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;

        if (isMenuOpen)
        {
            menuUI.Toggle(isMenuOpen);
            SetCurrentUIType(UIType.Menu);
            previousButton = eventSystem.currentSelectedGameObject;
            eventSystem.SetSelectedGameObject(menuUI.GetFirstButton());
            Time.timeScale = 0; //pause the game
            PlayersHandler.Instance.DisablePlayers(player);
        }
        else
        {
            menuUI.Toggle(isMenuOpen);
            SetCurrentUIType(previousType);
            eventSystem.SetSelectedGameObject(previousButton);
            Time.timeScale = 1f;
            PlayersHandler.Instance.EnablePlayers();
        }
    }


    public void ChangePrompt(InputPrompt input)
    {
        PlayerHUD.ChangePrompt(input);
        CraftingUI.ChangePrompt(input);
    }
}
