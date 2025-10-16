using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class PlayerUIManager : MonoBehaviour
{
    public PlayerHUD PlayerHUD => playerHUD;
    public MinigameUI MinigameUI => minigameUI;
    public InventoryUI InventoryUI => inventoryUI;
    public EquippedItemUI EquippedItemUI => equippedItemUI;
    public CraftingUI CraftingUI => craftingUI; 
    public DialogUI DialogUI => dialogUI;

    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private CraftingUI craftingUI;
    [SerializeField] private MinigameUI minigameUI;
    [SerializeField] private DialogUI dialogUI;
    [SerializeField] private PlayerHUD playerHUD;
    [SerializeField] private EquippedItemUI equippedItemUI;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Camera cam;

    private Player player;
    private bool isInventoryOpen = false;
    private bool isDialogOpen = false;
    private bool interactionActive = true; //used for DialogManager
    private bool isCraftingOpen = false;

    private enum UIType { None, Inventory, Crafting, Minigame, Menu, Dialog, Tutorial };
    UIType currentType = UIType.None;
    UIType previousType = UIType.None;


    void Start()
    {
        craftingUI.Toggle(false);
        minigameUI.Toggle(false);
        dialogUI.Toggle(false);
        inventoryUI.Toggle(false);
        cam = GetComponentInParent<Camera>();
    }

    public void SetPlayer(Player player) { this.player = player; }

    public void ToggleInventoryTrigger(InputAction.CallbackContext context)
    {
        if (context.performed) ToggleInventory();
    }
    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryUI.Toggle(isInventoryOpen);
        player.ToggleUI(isInventoryOpen);
        
        if (isInventoryOpen)
         {
             CloseOtherUIs(UIType.Inventory);
             inventoryUI.Toggle(isInventoryOpen);
             SetCurrentUIType(UIType.Inventory);
             eventSystem.SetSelectedGameObject(inventoryUI.GetFirstButton());
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

        if (currentUI != UIType.Dialog)
        {
            isDialogOpen = false;
            dialogUI.Toggle(isDialogOpen);
        }


        if (currentUI == UIType.None)
        {
            player.ToggleUI(false);
            //questUI.Toggle(true);
            equippedItemUI.Toggle(true);
            SetCurrentUIType(UIType.None);
        }
        else
        {
            //ToggleCursor(true);
            player.ToggleUI(true);
            equippedItemUI.Toggle(false);
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
        if (show)
        {
            eventSystem.SetSelectedGameObject(dialogUI.GetFirstButton());
        }
        else
        {
            eventSystem.SetSelectedGameObject(null);
        }
        interactionActive = show;
    }

    public void ToggleCrafting()
    {
        isCraftingOpen = !isCraftingOpen;

        if (isCraftingOpen)
        {
            CloseOtherUIs(UIType.Crafting);
            craftingUI.Toggle(isCraftingOpen);
            SetCurrentUIType(UIType.Crafting);
            EventSystem.current.SetSelectedGameObject(craftingUI.GetFirstButton());
        }
        else
        {
            CloseOtherUIs(UIType.None);
        }
    }
}
