using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private InventoryUI inventoryUI;
    private CraftingUI craftingUI;
    private ThirdPersonMovement player;
    private UIControls controls;

    private bool isInventoryOpen = false;
    private bool isCraftingOpen = false;
    //private bool isMenuOpen = false;

    private enum UIType { None, Inventory, Crafting, Menu };

    private void Awake()
    {
        controls = new UIControls();

        controls.ToggleUI.Inventory.performed += ctx => ToggleInventory();
        controls.ToggleUI.Crafting.performed += ctx => ToggleCrafting();
        //controls.UI.ToggleMenu.performed += ctx => ToggleMenu();
    }

    void Start()
    {
        inventoryUI = InventoryUI.Instance;
        craftingUI = CraftingUI.Instance;
        player = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<ThirdPersonMovement>();
        ToggleCursor(false);
        CloseOtherUIs(UIType.None);
    }

    private void OnEnable()
    {
        controls.ToggleUI.Enable();
    }

    private void OnDisable()
    {
        controls.ToggleUI.Disable();
    }
    private void ToggleCrafting()
    {
        isCraftingOpen = !isCraftingOpen;
        
        if(isCraftingOpen)
        {
            CloseOtherUIs(UIType.Crafting);
            craftingUI.Toggle(isCraftingOpen);
        }
        else
        {
            CloseOtherUIs(UIType.None);
        }
    }

    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;

        if(isInventoryOpen)
        {
            CloseOtherUIs(UIType.Inventory);
            inventoryUI.Toggle(isInventoryOpen);
        }
        else
        {
            CloseOtherUIs(UIType.None); 
        }
    }

    private void CloseOtherUIs(UIType currentUI)
    {
        if(currentUI != UIType.Inventory)
        {
            isInventoryOpen = false;
            inventoryUI.Toggle(isInventoryOpen);
        }

        if(currentUI != UIType.Crafting)
        {
            isCraftingOpen = false;
            craftingUI.Toggle(isCraftingOpen);
        }

        if(currentUI == UIType.None)
        {
            ToggleCursor(false);
            player.ToggleUI(false);
        }
        else
        {
            ToggleCursor(true);
            player.ToggleUI(true);
        }
    }

    private void ToggleCursor(bool showCursor)
    {
        Cursor.visible = showCursor;
        Cursor.lockState = showCursor ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
