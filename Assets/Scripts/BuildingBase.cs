using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BuildingBase : MonoBehaviour,  IMyUpdate
{
    [SerializeField] private CraftingRecipe.BuildingType buildingType;
   
    void IMyUpdate.MyUpdate()
    {
        if (Input.GetButtonDown("Interact"))
        {
            UIManager.Instance.ToggleCrafting();
            if (UIManager.Instance.isCraftingOpen)
            {
                CraftingUI.Instance.UpdateRecipeList(buildingType);
            }
            else
            {
                CraftingUI.Instance.UpdateRecipeList(CraftingRecipe.BuildingType.None);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerInteraction"))
        {
            //Add UI
            UpdateManager.Instance.AddUpdatable(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerInteraction"))
        {
            //Remove UI 
            UpdateManager.Instance.RemoveUpdatable(this);
        }
    }
   
}
