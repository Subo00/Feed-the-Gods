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

    protected ItemManager itemManager;

    protected override void Start()
    {
        lastInteractionTime = -interactionCooldown;
        itemManager = ItemManager.Instance;
        
        base.Start();
    }

    protected override void OnUpdate()
    {
        if (Time.time - lastInteractionTime < interactionCooldown)
        {
            float difference = interactionCooldown - (Time.time - lastInteractionTime);
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
        lastInteractionTime = Time.time;
        inUse = false;
    }
}
