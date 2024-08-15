using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Runtime.CompilerServices;

public class SourceBase : Interactable
{
    [System.Serializable]
    public struct ResourceDrop
    {
        public ItemData item;
        public uint value;
        [Range(0.01f, 1f)]
        public float dropChance;
    }

    [SerializeField]
    protected List<ResourceDrop> resourceDrops;

    //Interaction variables
    protected float lastInteractionTime;
    [SerializeField] protected float interactionCooldown = 6f;
    [SerializeField] private MinigameManager.MinigameType minigameType;

    //Managers
    protected ItemManager itemManager;
    protected TimeManager timeManager;

    private string savepath;

    protected override void Start()
    {
        savepath = "Test/" + SceneController.Instance.GetCurrentSceneName() + ".es3";

        LoadTime();

        itemManager = ItemManager.Instance;
        timeManager = TimeManager.Instance;

        base.Start();
    }

    protected override void OnUpdate()
    {
        if (timeManager.GetTime() - lastInteractionTime < interactionCooldown)
        {
            float difference = interactionCooldown - (timeManager.GetTime() - lastInteractionTime);
            uiManager.ShowTimeOnObject(dropPoint, difference);
        }
        else if (Input.GetButtonDown("Interact"))
        {
            MinigameManager.Instance.SetOnFinishMinigame(DropResource);
            MinigameManager.Instance.StartMinigame(minigameType);
            inUse = true;
        }
        else
        {
            CommonLogic();
        }
    }

    public virtual void DropResource(float dummy)
    {
        //set a cooldown 
        lastInteractionTime = timeManager.GetTime();
        inUse = false;
    }

    private void OnDisable()
    {
        SaveTime();
    }

    private void LoadTime()
    {
        if(ES3.KeyExists(name, savepath))
        {
            lastInteractionTime = ES3.Load<float>(name, savepath);
        }else
        {
            lastInteractionTime = -interactionCooldown;
            SaveTime();
        }
    }

    private void SaveTime()
    {
        ES3.Save(name, lastInteractionTime, savepath);
    }
}
