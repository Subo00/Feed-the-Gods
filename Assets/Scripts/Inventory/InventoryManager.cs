using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : InventoryManagerBase
{
    public bool CanCraft(CraftingRecipe recipe, uint numToCraft = 1)
    {

        for(int i = 0; i < recipe.ingredients.Length; i++)
        {
            uint ingredientID = recipe.ingredients[i].item.id;

            uint neededquantity = recipe.ingredients[i].quantity * numToCraft;
            uint quantity = 0;

            //first we see if the item is in equipped stack
            if (equippedItemStack != null && equippedItemStack.getItemData().id == ingredientID)
            {
                quantity += equippedItemStack.getCurrentStack();
            }

            //second we see if we have the said item in the inventory
            List<uint> list = ContainsId(ingredientID);
            if(list.Count == 0 && quantity < neededquantity) return false;

            //now we see if we have enough of it per wanted number of crafted item
            foreach (uint index in list)
            {
                quantity += items[(int)index].getCurrentStack();
            }
            if (quantity < neededquantity)
            {
                return false;
            }
        }

        //if we have enough of everything, we can craft it 
        return true;
    }

    //Achtung, the function doesn't check if it can be removed, it just removes it. 
    public void RemoveIngredients(CraftingRecipe recipe, uint numToCraft = 1)
    {
        for (int i = 0; i < recipe.ingredients.Length; i++)
        {
            RemoveItemQuantity(recipe.ingredients[i].item, recipe.ingredients[i].quantity * numToCraft);
        }
    }

   
    public void LoadPlayerData(PlayerData playerData)
    {
        foreach (KeyValuePair<uint, uint> entry in playerData.itemsStack)
        {
            ItemData itemData = ItemManager.GetItem(entry.Key);
            if (itemData == null) continue;

            ItemStack newStack = new ItemStack(itemData, entry.Value);

            // First item goes into the equipped slot, rest go into the inventory list
            if (equippedItemStack == null)
                SwitchEquippedItemStack(newStack);
            else
                AddStack(newStack);
        }
    }
    
    public void SavePlayerData(PlayerData playerData)
    {
        playerData.itemsStack.Clear();

        if (equippedItemStack != null)
            playerData.itemsStack[equippedItemStack.getItemData().id] = equippedItemStack.getCurrentStack();

        foreach (ItemStack stack in items)
            playerData.itemsStack[stack.getItemData().id] = stack.getCurrentStack();
    }

    public override void LoadData(GameData data)
    {
        if (playerIndex < 0 || !data.players.ContainsKey(playerIndex)) return;

        PlayerData playerData = data.players[playerIndex];

        foreach (KeyValuePair<uint, uint> entry in playerData.itemsStack)
        {
            ItemData itemData = ItemManager.GetItem(entry.Key);
            if (itemData == null) continue;

            ItemStack newStack = new ItemStack(itemData, entry.Value);

            // First item goes into the equipped slot, rest go into the inventory list
            if (equippedItemStack == null)
                SwitchEquippedItemStack(newStack);
            else
                AddStack(newStack);
        }
    }

    public override void SaveData(ref GameData data)
    {
        if (playerIndex < 0) return;

        if (!data.players.ContainsKey(playerIndex))
            data.players[playerIndex] = new PlayerData();

        data.players[playerIndex].itemsStack.Clear();

        if (equippedItemStack != null)
            data.players[playerIndex].itemsStack[equippedItemStack.getItemData().id] = equippedItemStack.getCurrentStack();

        foreach (ItemStack stack in items)
            data.players[playerIndex].itemsStack[stack.getItemData().id] = stack.getCurrentStack();
    }


}
