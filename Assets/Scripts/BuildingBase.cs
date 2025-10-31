using SmallHedge.SoundManager;
using UnityEngine;

public class BuildingBase : Interactable, RecipeSpawner
{
    [SerializeField] private BuildingType buildingType;
    protected ItemManager itemManager;

    public override void OnInteract(Player player)
    {
        if (inUse) return;
        inUse = true;

        interactingPlayer = player;
        player.MinigameManager.SetRecipeSpawner(this);
        player.CraftingManager.SetRecipeSpawner(this);

        player.ClearInteract();
        player.SetDelayedInteract(OnInteracted);
        //change all of this
        //CraftingUI.Instance.UpdateRecipeList(buildingType);
       //uiManagerChild.ToggleCrafting();
        //MinigameManager.Instance.SetRecipeSpawner(this);
    }

    public void OnInteracted()
    {
        interactingPlayer.PlayerUI.CraftingUI.UpdateRecipeList(buildingType);
        interactingPlayer.PlayerUI.ToggleCrafting();
        interactingPlayer.ClearInteract();
    }
    protected override void OnUpdate()
    {
        //inUse = uiManagerChild.isCraftingOpen || uiManagerChild.isMinigameOpen;


        CommonLogic();
    }

    protected override void Start()
    {
        itemManager = ItemManager.Instance;
        base.Start();
    }

    public void SpawnRecipe(CraftingRecipe recipe, uint numOfSpawnes)
    {
        uint itemID = recipe.craftingResult.item.id;
        uint quantity = recipe.craftingResult.quantity * numOfSpawnes;

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
