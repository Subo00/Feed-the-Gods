using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Camera mainCamera;
    private LayerMask inventoryLayer;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        inventoryLayer = LayerMask.GetMask("Inventory");
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

    private void ToggleCursor(bool showCursor)
    {
        Cursor.visible = showCursor;
        Cursor.lockState = showCursor ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
