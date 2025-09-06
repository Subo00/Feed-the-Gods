using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public PlayerHUD PlayerHUD => playerHUD;
    public InventoryUI InventoryUI => inventoryUI;

    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private CraftingUI craftingUI;
    [SerializeField] private MinigameUI minigameUI;
    [SerializeField] private DialogUI dialogUI;
    [SerializeField] private PlayerHUD playerHUD;
    [SerializeField] private EquippedItemUI equippedItemUI;

    void Start()
    {
        //craftingUI.Toggle(false);
       // minigameUI.Toggle(false);
        //dialogUI.Toggle(false);
        //inventoryUI.Toggle(false);
    }

    
    public void ToggleInventory()
    {
        /* isInventoryOpen = !isInventoryOpen;

         if (isInventoryOpen)
         {
             CloseOtherUIs(UIType.Inventory);
             inventoryUI.Toggle(isInventoryOpen);
             SetCurrentUIType(UIType.Inventory);
             EventSystem.current.SetSelectedGameObject(inventoryUI.GetFirstButton());
         }
         else
         {
             CloseOtherUIs(UIType.None);
         }*/
    }
    /*
    private void CloseOtherUIs(UIType currentUI)
    {
        if (currentUI != UIType.Inventory)
        {
            isInventoryOpen = false;
            inventoryUI.Toggle(isInventoryOpen);
        }

        if (currentUI == UIType.None)
        {
            ToggleCursor(false);
            playerMovement.ToggleUI(false);
            questUI.Toggle(true);
            SetCurrentUIType(UIType.None);
        }
        else
        {
            //ToggleCursor(true);
            playerMovement.ToggleUI(true);
        }
    }

    */
}
