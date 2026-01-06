using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonLeafManager : MonoBehaviour, ISeasonListener
{
    [SerializeField] private Material treeBranches;
    [SerializeField] private Color[] colors;
    public void OnSeasonChanged(Season currentSeason)
    {
        treeBranches.color = colors[(int)currentSeason];
    }

    private void Start()
    {
        SeasonManager.Instance.AddSeasonListener(this);
    }

    private void OnDisable()
    {
        SeasonManager.Instance.RemoveSeasonListener(this);
    }
}
