using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance;
    public enum MinigameType { None, Stomping, Gathering, HeadHit };

    [SerializeField] private GameObject minigameStompingPrefab;
    [SerializeField] private GameObject minigameGatheringPrefab;
    [SerializeField] private GameObject minigameHeadHitPrefab;

    private Minigame currentMinigame;
    private GameObject currentMinigamePrefab = null;

    private UIManagerChild uiManagerChild;
    private CraftingManager craftingManager;
    private MinigameUI minigameCanvas;

    private System.Action<float> onFinishMinigameSource;
    private CraftingRecipe recipe = null;
    private RecipeSpawner recipeSpawner = null;
    private bool? isSource = null;

    private void Awake()
    {
        if (Instance != null)
        {
            this.gameObject.SetActive(false);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        //make sure there is a game object wit the MinigameUI script attached
        minigameCanvas = GameObject.FindObjectOfType<MinigameUI>();
        uiManagerChild = UIManagerChild.Instance;
        craftingManager = CraftingManager.Instance;
    }

    public void SetOnFinishMinigame(System.Action<float> action)
    {
        onFinishMinigameSource = action;
        isSource = true;
    }

    public void SetOnFinishMinigame(CraftingRecipe recipe)
    {
        this.recipe = recipe;
        isSource = false;
    }

    public void StartMinigame(MinigameType minigameType, uint valueUint = 1)
    {
        
        switch (minigameType)
        {
            case MinigameType.Stomping:
                currentMinigamePrefab = Instantiate(minigameStompingPrefab, minigameCanvas.transformObject.transform);
                break;
            case MinigameType.Gathering:
                currentMinigamePrefab = Instantiate(minigameGatheringPrefab, minigameCanvas.transformObject.transform);
                break;
            case MinigameType.HeadHit:
                currentMinigamePrefab = Instantiate(minigameHeadHitPrefab, minigameCanvas.transformObject.transform);
                break;
                // Add other minigame types here
        }

        if (currentMinigamePrefab != null)
        {
            currentMinigame = currentMinigamePrefab.GetComponent<Minigame>();
            uiManagerChild.ToggleMinigame();
            currentMinigame.StartMinigame(valueUint);
        }
    }

    public void EndMinigame(float success)
    {
        currentMinigame.EndMinigame();
        uiManagerChild.ToggleMinigame();
        currentMinigame = null;

        Destroy(currentMinigamePrefab);
        currentMinigamePrefab=null;

        //if success then drop reward 
        if(success > 0f)
        {
            if(isSource == true)
            {
                onFinishMinigameSource(success);
            }
            else
            {
                recipeSpawner.SpawnRecipe(recipe/*,(uint) success*/);
            }
        }
        else
        {
            Debug.Log("Minigame failed");
        }

        isSource = null;
        recipe = null;
    }

    public void SetRecipeSpawner(RecipeSpawner spawner)
    {
        recipeSpawner = spawner;
    }

    public void SpawnFromSpawner()
    {
        recipeSpawner.SpawnRecipe(recipe);
    }
}
