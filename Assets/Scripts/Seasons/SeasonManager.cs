using System;
using UnityEngine;


public enum Season { PLANTING, HARVEST, WINTER }
public class SeasonManager : MonoBehaviour
{
    public static SeasonManager Instance;

    public event Action OnSeasonChanged;
    public Season currentSeason = Season.PLANTING;

    public void ChangeSeason()
    {
        Debug.Log(currentSeason);
        OnSeasonChanged?.Invoke();
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}

