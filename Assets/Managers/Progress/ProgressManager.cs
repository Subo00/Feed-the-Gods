using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance;

    private CraftingRecipe[] craftingRecipes;
    [SerializeField] private List<int> discoveredRecipes;
    [SerializeField] private float[,] states = new float[1, 4];
    private float[,] baseStates = new float[1, 4];
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
        }
        
        Instance = this;

        craftingRecipes = Resources.LoadAll<CraftingRecipe>("Crafting");
    }

    private void Start()
    {
        //load discoveredRecipes
       // states = ES3
         //   .Load("testArray", baseStates);
      /*  for(int i = 0; i < 4; i++)
        {
            Debug.Log("i = " + i + ": " + states[0, i]);
        }*/
        
    }

    public List<CraftingRecipe> getDiscoveredRecipes()
    {
        List<CraftingRecipe> returnRecipes = new List<CraftingRecipe>();
        for(int i = 0; i < discoveredRecipes.Count; i++)
        {
            returnRecipes.Add(craftingRecipes[discoveredRecipes[i]]);
        }

        return returnRecipes;
    }

    public void AddToDiscoveredRecipes(int recipeIndex)
    {
        if(discoveredRecipes.Contains(recipeIndex))
        {
            return;
        }    
        discoveredRecipes.Add(recipeIndex);
        //save the list 
    }

    public void OnApplicationQuit()
    {
        //ES3.Save("testArray", states);
    }

    public void SaveTest()
    {
        states[0, 0] = 3f;
    }
}
