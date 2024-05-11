using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceBase : MonoBehaviour, IInteractable, IMyUpdate
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

    private ItemManager itemManager;

    private void Start()
    {
        lastInteractionTime = -interactionCooldown;
        itemManager = ItemManager.Instance;
    }

    void IInteractable.StartInteraction() { }
    void IInteractable.DisruptInteraction() { }
    void IInteractable.EndInteraction() { }
    void IMyUpdate.MyUpdate()
    {
        if(Input.GetButtonDown("Interact") )
        {
            if(Time.time - lastInteractionTime > interactionCooldown)
            {
                lastInteractionTime = Time.time;
                DropResource();
            }
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
            UpdateManager.Instance.RemoveUpdatable(this);
        }
    }
    public void DropResource()
    {
        foreach(var resource in resourceDrops)
        {
            GameObject itemToDrop = itemManager.GetGameObject(resource.item.id);
            GameObject drop = Instantiate(itemToDrop, transform.position, Quaternion.identity);

            Vector3 rotation = new Vector3(Random.Range(-1f, 1f),0 , Random.Range(-1f, 1f));

            drop.GetComponent<ItemPickUp>().LaunchInDirection(rotation);
        }
    }
}
