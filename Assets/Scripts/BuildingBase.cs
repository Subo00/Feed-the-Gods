using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BuildingBase : Interactable, RecipeSpawner
{
    [SerializeField] private BuildingType buildingType;
    protected ItemManager itemManager;
    protected override void OnUpdate()
    {
        inUse = UIManager.Instance.isCraftingOpen || UIManager.Instance.isMinigameOpen;

        if (Input.GetButtonDown("Interact") & !inUse)
        {
            CraftingUI.Instance.UpdateRecipeList(buildingType);
            UIManager.Instance.ToggleCrafting();
            MinigameManager.Instance.SetRecipeSpawner(this);
        }

        CommonLogic();
    }

    protected override void Start()
    {
        itemManager = ItemManager.Instance;
        base.Start();
    }

    public void SpawnRecipe(CraftingRecipe recipe)
    {
        uint itemID = recipe.craftingResult.item.id;
        uint quantity = recipe.craftingResult.quantity;

        //Dropt it from object's position 
        Vector3 objectPos = transform.position + transform.forward * 3; //magic number 3
        Quaternion objectRot = transform.rotation;

        while (quantity > 0)
        {
            //TODO: CHANGE THIS A BIT 
            //Add Instantiate 
            GameObject itemToDrop = itemManager.GetGameObject(itemID);
            GameObject drop = Instantiate(itemToDrop, objectPos, objectRot);
            drop.GetComponent<ItemPickUp>().LaunchInDirection(transform.forward + transform.up, 5f); //magic number 5
            SoundManager.PlaySound(SoundType.ItemSpawned);

            quantity--;
        }
    }
}
