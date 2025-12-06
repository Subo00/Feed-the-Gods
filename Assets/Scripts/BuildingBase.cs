using SmallHedge.SoundManager;
using UnityEngine;

public class BuildingBase : Interactable, RecipeSpawner
{
    [SerializeField] private BuildingType buildingType;
    [SerializeField] private GameObject interactingObject;
    [SerializeField] private Vector3 interactingOffset;

    protected ItemManager itemManager;

    public override void OnInteract(Player player)
    {
        if (inUse) return;
        inUse = true;

        interactingPlayer = player;
        interactingPlayer.MinigameManager.SetRecipeSpawner(this);
        interactingPlayer.MinigameManager.SetOnFailMinigame(ResetInUse);
        interactingPlayer.MinigameManager.SetOnEndAction(ResetInUse);
        interactingPlayer.CraftingManager.SetRecipeSpawner(this);

        interactingPlayer.ClearInteract();
        interactingPlayer.SetDelayedInteract(OnInteracted);
        interactingObject.transform.SetParent(interactingPlayer.transform);
        interactingObject.transform.localPosition = interactingOffset;
        interactingObject.SetActive(true);
        //change all of this
        //CraftingUI.Instance.UpdateRecipeList(buildingType);
        //uiManagerChild.ToggleCrafting();
        //MinigameManager.Instance.SetRecipeSpawner(this);
    }

    public void OnInteracted()
    {
        interactingPlayer.ClearInteract();
        interactingPlayer.PlayerUI.CraftingUI.UpdateRecipeList(buildingType);
        interactingPlayer.PlayerUI.ToggleCrafting();
        interactingPlayer.AddOnCancle(ResetInUse);
    }
    protected override void OnUpdate()
    {
        //inUse = uiManagerChild.isCraftingOpen || uiManagerChild.isMinigameOpen;


        CommonLogic();
    }

    public void ResetInUse()
    {
        interactingPlayer.SetInteract(OnInteract);
        interactingObject.transform.SetParent(this.transform);
        interactingObject.SetActive(false);
        inUse = false;
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
