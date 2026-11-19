using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class CraftingUI : BaseUI, UIPrompt
{
    [SerializeField] InventorySlot[] inventorySlots;
    [SerializeField] TMP_Text numText;
    [SerializeField] Transform panelTransform;
    [SerializeField] CraftingItemButtonUI craftingButtonUI;
    [SerializeField] Image confirmButton;
    [SerializeField] Image cancleButton;
    
    private InventoryManager inventoryManager;

    List<CraftingRecipe> recipes;
    private int counter = 0;
    private uint numToCraft = 1;

    private NavigationButton[] navigationButtons;
    enum Navigation { Right, Left, Up, Down};

    private float debounceTime = 0.15f;
    private float lastInputTime = 0f;
    private float deadzone = 0.5f;
    
    private void Start()
    {
        //make sure the buttons are properly aligned in the editor 
        navigationButtons = new NavigationButton[4];
        navigationButtons = gameObject.GetComponentsInChildren<NavigationButton>();
    }

    public void Bind(Player player)
    {
        inventoryManager = player.Inventory;
        craftingButtonUI.Initialize(player.CraftingManager.CraftRecipe);
    }


    public void OnMove(InputAction.CallbackContext context)
    {
     
        if (!canvas.enabled || !context.performed) return;

        // Debounce: do not process too rapidly
        if (Time.time - lastInputTime < debounceTime) return;
        lastInputTime = Time.time;

        Vector2 input = context.ReadValue<Vector2>();

        if (Mathf.Abs(input.x) > deadzone)
        {
            MoveHorizontal(input.x > 0);
            return;
        }

        if (Mathf.Abs(input.y) > deadzone)
        {
            MoveVertical(input.y > 0);
            return;
        }
    }
   
    public void MoveVertical(bool positive)
    {
        if(positive)
        {
            if(numToCraft + 1 <= 10)
            {
                navigationButtons[(int)Navigation.Up].Flicker();
                numToCraft++;
            }
        }else
        {
            if(numToCraft - 1 >= 1)
            {
                navigationButtons[(int)Navigation.Down].Flicker();
                numToCraft--;
            }
        }

        craftingButtonUI.SetNumToCraft(numToCraft);
        ChangeDescription();
    }

    public void MoveHorizontal(bool positive)
    {
        if(positive)
        {
            if(counter +1 < recipes.Count)
            {
                navigationButtons[(int)Navigation.Right].Flicker();
                counter++;
            }
        } else
        {
            if (counter - 1 >= 0)
            {
                navigationButtons[(int)Navigation.Left].Flicker();
                counter--;
            }
        }

        craftingButtonUI.SetRecipe(recipes[counter]);
        RefreshDescription();
    }

    public void ChangeDescription()
    {
        //clear the ingredients list
        foreach (InventorySlot slot in inventorySlots)
        {
            slot.Clear();
        }

        CraftingRecipe recipe = recipes[counter];

        //changes the ingredients list 
        for (int i = 0; i < recipe.ingredients.Length; i++)
        {
            ItemData itemData = recipe.ingredients[i].item;    //ingreiends per recipe
            uint neededAmount = recipe.ingredients[i].quantity * numToCraft;
            ItemStack tmpStack = new ItemStack(itemData, neededAmount);
            inventorySlots[i].Set(tmpStack);
            inventorySlots[i].SetNeededAmount(inventoryManager.CurrentItemCount(itemData), neededAmount);
        }

        numText.text = "" + numToCraft;

        //if it can be crafted it can be clicked 
        craftingButtonUI.SetClickable(inventoryManager.CanCraft(recipe, numToCraft));
    }

    public void UpdateRecipeList(BuildingType buildingType)
    {
        recipes = ProgressManager.Instance.getDiscoveredRecipes();
        List < CraftingRecipe > recipesToRemove = new List < CraftingRecipe >();

        foreach (var recipe in recipes)
        {
            if(recipe.buildingRequired != buildingType)
            {
                recipesToRemove.Add(recipe);
            }
        }

        foreach (var recipe in recipesToRemove)
        {
            recipes.Remove(recipe);
        }

        counter = 0;
        craftingButtonUI.SetRecipe(recipes[counter]);
        RefreshDescription();
    }

    public void RefreshDescription()
    {
        numToCraft = 1;
        craftingButtonUI.SetNumToCraft(numToCraft);
        ChangeDescription();
    }

    public override GameObject GetFirstButton()
    {
        return craftingButtonUI.gameObject;
    }

    public void SetPrompt(InputPrompt input)
    {
        switch (input)
        {
            case InputPrompt.WASD:
                confirmButton.sprite = UIPromptManager.instance.ConfirmWASD;
                cancleButton.sprite = UIPromptManager.instance.CancelWASD;
                break;
            case InputPrompt.IJKL:
                confirmButton.sprite = UIPromptManager.instance.ConfirmIJKL;
                cancleButton.sprite = UIPromptManager.instance.CancelIJKL;
                break;
            case InputPrompt.XBOX:
                confirmButton.sprite = UIPromptManager.instance.ConfirmXBOX;
                cancleButton.sprite = UIPromptManager.instance.CancelXBOX;
                break;
            case InputPrompt.PLAYSTATION:
                confirmButton.sprite = UIPromptManager.instance.ConfirmPS;
                cancleButton.sprite = UIPromptManager.instance.CancelPS;
                break;
        }
    }
}
