using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BuildingBase : Interactable
{
    [SerializeField] private BuildingType buildingType;

    protected override void OnUpdate()
    {
        inUse = UIManager.Instance.isCraftingOpen || UIManager.Instance.isMinigameOpen;

        if (Input.GetButtonDown("Interact"))
        {
            UIManager.Instance.ToggleCrafting();

            if (UIManager.Instance.isCraftingOpen)
            {
                CraftingUI.Instance.UpdateRecipeList(buildingType);
            }
            else
            {
                CraftingUI.Instance.UpdateRecipeList(BuildingType.None);
            }
        }

        CommonLogic();
    }
   
}
