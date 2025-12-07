using System;
using System.Collections.Generic;
using UnityEngine;


public enum Season { SPRING, SUMMER, FALL, WINTER }

public interface ISeasonListener
{
    void OnSeasonChanged(Season currentSeason);
}

public class SeasonManager : MonoBehaviour
{
    public static SeasonManager Instance;

    public event Action OnSeasonChanged;
    public Season currentSeason = Season.SPRING;

    private List<ISeasonListener> seasonListeners = new List<ISeasonListener> ();

    public void AddSeason(ISeasonListener season) => seasonListeners.Add(season);
    public void RemoveSeason(ISeasonListener season) => seasonListeners.Remove(season);

    public void ChangeSeason()
    {
        currentSeason = (Season)(((int)currentSeason + 1) % 4);
        Debug.Log(currentSeason);
        foreach(var season in seasonListeners)
            season?.OnSeasonChanged(currentSeason);
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

