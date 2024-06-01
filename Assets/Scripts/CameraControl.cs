using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Camera mainCamera;
    private LayerMask inventoryLayer;
    private LayerMask craftingLayer;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        inventoryLayer = LayerMask.GetMask("Inventory");
        craftingLayer = LayerMask.GetMask("Crafting");
        ToggleCursor(false);
    }


    public void ToggleInventory(bool showInventory)
    {
        ToggleCursor(showInventory);

        if (showInventory)
        {
            mainCamera.cullingMask |= inventoryLayer;
        }
        else
        {
            mainCamera.cullingMask &= ~inventoryLayer;
        }
    }

    public void ToggleCrafting(bool showCrafting)
    {
        ToggleCursor(showCrafting);
        if (showCrafting)
        {
            mainCamera.cullingMask |= craftingLayer;
        }
        else
        {
            mainCamera.cullingMask &= ~craftingLayer;
        }
    }

    private void ToggleCursor(bool showCursor)
    {
        Cursor.visible = showCursor;
        Cursor.lockState = showCursor ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
