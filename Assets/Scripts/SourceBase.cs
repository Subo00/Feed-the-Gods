using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Runtime.CompilerServices;

public class SourceBase : MonoBehaviour,  IMyUpdate
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
    List<ResourceDrop> resourceDrops;

    //Interaction variables
    private float lastInteractionTime;
    public float interactionCooldown = 6f;

    [SerializeField] private MinigameManager.MinigameType minigameType;
    [SerializeField] private Transform dropPoint;
    private bool inMinigame = false;

    private ItemManager itemManager;
    private UIManager uiManager;

    private void Start()
    {
        lastInteractionTime = -interactionCooldown;
        itemManager = ItemManager.Instance;
        uiManager = UIManager.Instance;
    }

    void IMyUpdate.MyUpdate()
    {
        if(Time.time - lastInteractionTime < interactionCooldown)
        {
            float difference = interactionCooldown - (Time.time - lastInteractionTime);
            uiManager.ShowTimeOnObject(dropPoint, Mathf.Round(difference * 10) / 10);
        }
        else if(Input.GetButtonDown("Interact"))
        {
            MinigameManager.Instance.SetOnFinishMinigame(DropResource);
            MinigameManager.Instance.StartMinigame(minigameType);
            inMinigame = true;
        }
        else if(inMinigame)
        {
            uiManager.HideInteraction();
        }
        else
        {
            uiManager.ShowInteractionOnObject(dropPoint);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerInteraction"))
        {
            //Add UI
            UpdateManager.Instance.AddUpdatable(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("PlayerInteraction"))
        {
            //Remove UI 
            UIManager.Instance.HideInteraction();
            UpdateManager.Instance.RemoveUpdatable(this);
        }
    }
    public void DropResource(float dummy)
    {
        float randNum = Random.Range(0.01f, 1f);

        foreach(var resource in resourceDrops)
        {
            if (resource.dropChance < randNum) continue; 

            GameObject itemToDrop = itemManager.GetGameObject(resource.item.id);
            
           
            for(uint i = 0; i < resource.value; i++)
            {
                GameObject drop = Instantiate(itemToDrop, dropPoint.position, Quaternion.identity);
                Vector3 rotation = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                drop.GetComponent<ItemPickUp>().LaunchInDirection(rotation);
                Thread.Sleep(50);
            }
            
        }

        //set a cooldown 
        lastInteractionTime = Time.time;
        inMinigame = false;
    }
}
