using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantPrefabGetter : MonoBehaviour
{
    public static PlantPrefabGetter Instance;

    public GameObject[][] platns;
    public GameObject Level1Prefab;
    public GameObject Level2Prefab;
    public GameObject Level3Prefab;
    public GameObject Level4Prefab;

    public GameObject[] grassPrefabs;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject getPlant(int level)
    {
        switch (level)
        {
            case 1:
                return Level1Prefab;
            case 2:
                return Level2Prefab;
            case 3:
                return Level3Prefab;
            case 4:
                return Level4Prefab;
        }
        return null;
    }

    public GameObject getGrass(float percent)
    {
        if (percent <= 0.1f)
        {
            return grassPrefabs[0];
        }
        else if (percent <= 0.3f)
        {
            return grassPrefabs[1];
        }
        else if (percent <= 0.6f)
        {
            return grassPrefabs[2];
        }
        else
        {
            return grassPrefabs[3];
        }
    }
}
