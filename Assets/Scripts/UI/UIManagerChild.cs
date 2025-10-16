using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManagerChild : PlayerHUD //delete this script 
{
    public static UIManagerChild Instance;

    private InventoryUI inventoryUI;
    private CraftingUI craftingUI;
    private MinigameUI minigameUI;
    private DialogUI dialogUI;
    private QuestUI questUI;
    //private TutorialUI tutorialUI;
    private ThirdPersonMovement playerMovement;
    private UIControls controls;

    public bool isCraftingOpen = false;
    public bool isMinigameOpen = false;
    private bool isInventoryOpen = false;
    private bool isDialogOpen = false;
    private bool isMenuOpen = false;
    private bool interactionActive = true; //used for DialogManager


    private enum UIType { None, Inventory, Crafting, Minigame, Menu, Dialog, Tutorial };
    UIType currentType = UIType.None;
    UIType previousType = UIType.None;

    private GameObject previousButton = null;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        controls = new UIControls();

        controls.ToggleUI.Inventory.performed += ctx => { if (!IsImportatnTypeOpen()) ToggleInventory(); };
        controls.ToggleUI.Crafting.performed += ctx => { if(!isCraftingOpen) craftingUI.UpdateRecipeList(BuildingType.None); };
        controls.ToggleUI.Crafting.performed += ctx => { if (!IsImportatnTypeOpen()) ToggleCrafting(); };
        //controls.ToggleUI.Menu.performed += ctx =>  ToggleMenu();
        controls.ToggleUI.Cancle.performed += ctx => { if (!IsImportatnTypeOpen()) CloseOtherUIs(UIType.None); };

        controls.UI.Interact.performed += ctx => OnInteractPressed(); //enables UI interaction with the E button

        base.Awake();

        //PlayerHUD.Instance = this;
        HideInteraction();

        DontDestroyOnLoad(this.gameObject);
    }

    protected void Start()
    {
        /*
        inventoryUI = InventoryUI.Instance;
        craftingUI = CraftingUI.Instance;
        minigameUI = MinigameUI.Instance;
        dialogUI = DialogUI.Instance;
        questUI = QuestUI.Instance;*/
        //tutorialUI = TutorialUI.Instance;

        playerMovement = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<ThirdPersonMovement>();

        CloseOtherUIs(UIType.None);

        /*if(tutorialUI != null)
        {
            playerMovement.ToggleUI(true);
            EventSystem.current.SetSelectedGameObject(tutorialUI.GetFirstButton());
        }*/
    }

    private void OnEnable()
    {
        controls.ToggleUI.Enable();
        controls.UI.Enable();
    }

    private void OnDisable()
    {
        controls.ToggleUI.Disable();
        controls.UI.Disable();
    }



    public void ToggleCrafting()
    {
        isCraftingOpen = !isCraftingOpen;
        
        if(isCraftingOpen)
        {
            CloseOtherUIs(UIType.Crafting);
            craftingUI.Toggle(isCraftingOpen);
            SetCurrentUIType(UIType.Crafting);
            EventSystem.current.SetSelectedGameObject(craftingUI.GetFirstButton());
        }
        else
        {
            CloseOtherUIs(UIType.None);
        }
    }

   

   /* public override void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;

        if(isMenuOpen)
        {
            menuUI.Toggle(isMenuOpen);
            questUI.Toggle(false);
            SetCurrentUIType(UIType.Menu);
            previousButton = EventSystem.current.currentSelectedGameObject;
            EventSystem.current.SetSelectedGameObject(menuUI.GetFirstButton());
            Time.timeScale = 0; //pause the game
        }
        else
        {
            menuUI.Toggle(isMenuOpen);
            questUI.Toggle(true);
            SetCurrentUIType(previousType);
            EventSystem.current.SetSelectedGameObject(previousButton);
            Time.timeScale = 1f;
        }
    }
   */
    public void ToggleMinigame()
    {
        isMinigameOpen = !isMinigameOpen;

        if(isMinigameOpen)
        {
            CloseOtherUIs(UIType.Minigame);

            //set minigameUI's position to lastInteractPoint
            SetUIPosition(UIType.Minigame);

            minigameUI.Toggle(isMinigameOpen);
            SetCurrentUIType(UIType.Minigame);
            ToggleCursor(false);
        }
        else
        {
            CloseOtherUIs(UIType.None);
        }
    }    

    public void ShowDialogResponse(bool show)
    {
        if(show)
        {
            EventSystem.current.SetSelectedGameObject(dialogUI.GetFirstButton());
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        interactionActive = show;

    }

    public void ToggleDialog()
    {
        isDialogOpen = !isDialogOpen;

        if (isDialogOpen)
        {
            CloseOtherUIs(UIType.Dialog);
            SetUIPosition(UIType.Dialog);
            dialogUI.Toggle(isDialogOpen);
            SetCurrentUIType(UIType.Dialog);
            ToggleCursor(false);
            interactionActive = false;
        }
        else
        {
            CloseOtherUIs(UIType.None);
            interactionActive = true;
        }
    }

    private void CloseOtherUIs(UIType currentUI)
    {
        if (currentUI != UIType.Inventory)
        {
            isInventoryOpen = false;
            inventoryUI.Toggle(isInventoryOpen);
        }

        if (currentUI != UIType.Crafting)
        {
            isCraftingOpen = false;
            craftingUI.Toggle(isCraftingOpen);
        }

        if (currentUI != UIType.Minigame)
        {
            isMinigameOpen = false;
            minigameUI.Toggle(isMinigameOpen);
        }

        if(currentUI != UIType.Dialog)
        {
            isDialogOpen = false;
            dialogUI.Toggle(isDialogOpen);
        }

       /* if(currentUI != UIType.Menu)
        {
            isMenuOpen = false;
            menuUI.Toggle(isMenuOpen);
        }*/

        if(currentUI != UIType.Tutorial)
        {
       //     tutorialUI.Toggle(false);
        }

        if (currentUI == UIType.None)
        {
            ToggleCursor(false);
            playerMovement.ToggleUI(false);
            questUI.Toggle(true);
            SetCurrentUIType(UIType.None);
        }
        else
        {
            //ToggleCursor(true);
            playerMovement.ToggleUI(true);
        }
    }

    private void ToggleCursor(bool showCursor)
    {
        Cursor.visible = showCursor;
        Cursor.lockState = showCursor ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void SetCurrentUIType(UIType type)
    {
        previousType = currentType;
        currentType = type;
    }    

    private void SetUIPosition(UIType uiType)
    { 
        // Convert the world point to screen point
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(lastInteractPoint.position);

        // Check if the point is in front of the camera
        if (screenPoint.z > 0)
        {
            // Convert screen point to canvas space
            Vector2 canvasPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPoint, canvas.worldCamera, out canvasPos);

            switch(uiType)
            {
                case UIType.None:
                    return;
                case UIType.Minigame:
                    minigameUI.transformObject.GetComponent<RectTransform>().anchoredPosition = canvasPos;
                    return;
                case UIType.Dialog:
                    dialogUI.transformObject.GetComponent<RectTransform>().anchoredPosition = canvasPos;
                    return;
            }
        }

    }

    public bool IsImportatnTypeOpen()
    {
        switch (currentType)
        {
            case UIType.Menu:
            case UIType.Minigame:
            case UIType.Dialog:
                return true;
            default:
                return false;
        }
    }

    public void OnInteractPressed()
    {
        if(!isUIOpen() || !interactionActive) return;

        var currentSelected = EventSystem.current.currentSelectedGameObject;

        if (currentSelected != null)
        {
            var button = currentSelected.GetComponent<UnityEngine.UI.Button>();
            if (button != null)
            {
                button.onClick.Invoke();
            }
        }
    }

    private bool isUIOpen()
    {
        return isCraftingOpen || isDialogOpen || isInventoryOpen ||
               isMenuOpen || isMinigameOpen;
    }


    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;

        if (isInventoryOpen)
        {
            CloseOtherUIs(UIType.Inventory);
            inventoryUI.Toggle(isInventoryOpen);
            SetCurrentUIType(UIType.Inventory);
            EventSystem.current.SetSelectedGameObject(inventoryUI.GetFirstButton());
        }
        else
        {
            CloseOtherUIs(UIType.None);
        }
    }

}
