using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class PlayerUIManager : MonoBehaviour
{
    public PlayerHUD PlayerHUD => playerHUD;
    public MinigameUI MinigameUI => minigameUI;
    public InventoryUI InventoryUI => inventoryUI;
    public EquippedItemUI EquippedItemUI => equippedItemUI;
    public DialogUI DialogUI => dialogUI;

    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private CraftingUI craftingUI;
    [SerializeField] private MinigameUI minigameUI;
    [SerializeField] private DialogUI dialogUI;
    [SerializeField] private PlayerHUD playerHUD;
    [SerializeField] private EquippedItemUI equippedItemUI;
    [SerializeField] private EventSystem eventSystem;

    private Player player;
    private bool isInventoryOpen = false;

    private enum UIType { None, Inventory, Crafting, Minigame, Menu, Dialog, Tutorial };
    UIType currentType = UIType.None;
    UIType previousType = UIType.None;


    void Start()
    {
        //craftingUI.Toggle(false);
         minigameUI.Toggle(false);
        //dialogUI.Toggle(false);
        inventoryUI.Toggle(false);
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


}
