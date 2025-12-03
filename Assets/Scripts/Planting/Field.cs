using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : Interactable, Collector
{
    CollectorCollider collector;
    public ItemData appleData;
    public ItemData waterData;
    public uint neededNumber = 3;
    public uint fibonachi = 2;
    private uint currentNumber = 0;

    PlantPrefabGetter plantPrefabGetter;
    GameObject plantGO = null;

    private int currentLevel = 0;

    private const float radious = 2.5f;

    protected override void Start()
    {
        collector = GetComponentInChildren<CollectorCollider>();
        if (collector == null)
        {
            Debug.LogError("Collector not properly set up");
        }
        collector.SetNeededItems(new List<ItemStack> { new ItemStack(appleData, neededNumber) });
        plantPrefabGetter = PlantPrefabGetter.Instance;

        inUse = true;

        base.Start();
    }

    public void ReportBool(bool value)
    {
        currentNumber++;
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

            /*if (currentLevel != 1)
            {
                colliderPosition.x += Random.Range(-radious, radious);
                colliderPosition.z += Random.Range(-radious, radious);
                GameObject grassGO = Instantiate(plantPrefabGetter.getGrass(Random.Range(0f, 1f)), colliderPosition, playerRot);
            }*/
            uint tmp = neededNumber;
            neededNumber += fibonachi;
            fibonachi = tmp;

            if (currentLevel != 4)
            {
                currentNumber = 0;
                collector.SetNeededItems(new List<ItemStack> { new ItemStack(waterData, neededNumber) });
            }

        }
    }

    protected override void OnUpdate()
    {
        if (currentNumber < neededNumber)
        {
            //if (currentLevel != 4) 
            //uiManager.ShowProgressOnObjedct(dropPoint, (int)currentNumber, (int)neededNumber, currentLevel == 0 ? appleData.name : waterData.name);
        }
        CommonLogic();
    }

    public override void OnInteract(Player player)
    {
        throw new System.NotImplementedException();
    }
}

