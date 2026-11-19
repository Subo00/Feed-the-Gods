using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CraftingIngredient
{
    public ItemData item;
    public uint quantity;
}
public enum BuildingType { None, Vinery, Mill, FirePit, Pot }

[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Item/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public CraftingIngredient craftingResult;
    public CraftingIngredient[] ingredients = null;
    public BuildingType buildingRequired = BuildingType.None;
}
