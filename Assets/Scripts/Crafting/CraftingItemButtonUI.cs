using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingItemButtonUI : ButtonUI
{
    public CraftingRecipe recipe;

    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemName;

    private uint numToCraft = 1;

    public void Initialize(System.Action<CraftingRecipe, uint> onClick)
    {
        // Add functionality to the button 
        button.onClick.AddListener(() => { onClick(recipe, numToCraft); });
    }

    public void SetClickable(bool isClickable)
    {
        Image buttonImage = button.gameObject.GetComponent<Image>();
        var color = buttonImage.color;
        color.a = isClickable ? 1f : 0.5f;
        buttonImage.color = color;
    }

    public void SetRecipe(CraftingRecipe recipe)
    {
        // Change the look
        this.recipe = recipe;
        itemImage.sprite = recipe.craftingResult.item.sprite;
        itemName.text = LocalizationManager.Instance.GetLine(recipe.craftingResult.item.name);
    }

    public void SetNumToCraft(uint numToCraft)
    {
        this.numToCraft = numToCraft;
    }
}
