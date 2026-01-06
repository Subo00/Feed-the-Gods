using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : Interactable, Collector, ISeasonListener
{
    CollectorCollider collector;
    private uint currentNumber = 0;

    GameObject plantGO = null;

    //Plant spesific data
    private PlantData currentPlant;

    private int currentLevel = 0;
    private int numOfSeasons = 0;
    private bool isPlanted = false;
    private int step = 0;

    protected override void Start()
    {
        collector = GetComponentInChildren<CollectorCollider>();
        if (collector == null)
        {
            Debug.LogError("Collector not properly set up");
        }

        inUse = true;

        base.Start();
    }

    public void ReportBool(bool value)
    {
        /*currentNumber++;
        if (value)
        {
            currentLevel++;

            if (currentLevel == 5)
            {
                return;
            }

            //SoundManager.PlaySound(SoundType.LevelUP);

            if (plantGO != null)
            {
                plantGO.GetComponent<SourceBase>().BeforDestroy();
                Destroy(plantGO);
                plantGO = null;
            }
            else
            {
                //TempQuest.Instance.OnTreeSpawn();
            }


            Vector3 colliderPosition = collector.transform.position;
            colliderPosition.y = 0.01f;
            Quaternion playerRot = transform.rotation;

            plantGO = Instantiate(plantPrefabGetter.getPlant(currentLevel), colliderPosition, playerRot);

            if (currentLevel != 1)
            {
                colliderPosition.x += Random.Range(-radious, radious);
                colliderPosition.z += Random.Range(-radious, radious);
                GameObject grassGO = Instantiate(plantPrefabGetter.getGrass(Random.Range(0f, 1f)), colliderPosition, playerRot);
            }
            uint tmp = neededNumber;
            neededNumber += fibonachi;
            fibonachi = tmp;

            if (currentLevel != 4)
            {
                currentNumber = 0;
                collector.SetNeededItems(new List<ItemStack> { new ItemStack(waterData, neededNumber) });
            }

        }*/
    }

    protected override void OnUpdate()
    {
        CommonLogic();
    }

    public override void OnInteract(Player player)
    {
        throw new System.NotImplementedException();
    }

    public void SetPlantData(PlantData plant)
    {
        currentPlant = plant;
        isPlanted = true;
        step = GetStep();
        SetPlant();
    }

    private void SetPlant()
    {
        Vector3 colliderPosition = collector.transform.position;
        colliderPosition.y = -1f;
        Quaternion plotRot = transform.rotation;

        plantGO = Instantiate(currentPlant.growthStagePrefabs[currentLevel], colliderPosition, plotRot);
        SeasonManager.Instance.AddSeasonListener(this);
    }

    private void KillPlant()
    {
        Destroy(plantGO);
        plantGO = null;
        collector.SetIsFull();
        SeasonManager.Instance.RemoveSeasonListener(this);
    }
    public void OnSeasonChanged(Season currentSeason)
    {
        if (plantGO == null) return;

        if(currentSeason != currentPlant.plantingSeason + 1 && isPlanted)
        {
            KillPlant();
            isPlanted = false;

            return;
        }

        isPlanted = false;
        numOfSeasons++;
        if(numOfSeasons% step == 0 && currentLevel != currentPlant.growthStagePrefabs.Length - 2)
        {
            KillPlant();
            currentLevel++;
            SetPlant();
            collector.SetIsFull(true);
        }
        if(numOfSeasons == currentPlant.lifespan)
        {
            KillPlant();
            currentLevel = currentPlant.growthStagePrefabs.Length - 1;
            SetPlant();
            SeasonManager.Instance.RemoveSeasonListener(this);
            return;
        }
    }

    private int GetStep()
    {
        float step = currentPlant.growthStagePrefabs.Length - 2 / currentPlant.seasonsToGrow;
        Debug.Log("step:" + step);
        return (int)step;
    }
}

