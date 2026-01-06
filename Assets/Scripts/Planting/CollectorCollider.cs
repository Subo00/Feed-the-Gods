using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorCollider : MonoBehaviour
{
    //private List<ItemStack> neededItems = new List<ItemStack>();
    private Collector collector = null;
    private bool isFull = false;

    public void SetIsFull(bool full = false)
    {
        isFull = full;
    }

    private void Start()
    {
        collector = GetComponentInParent<Collector>();
        if (collector == null)
        {
            Debug.LogError("CollectorCollider's master not properly set up");
        }
    }

    //GameObject need to have a collider attached to it 
    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<PlantPickUp>() || isFull)
            return;
        
        PlantData plant = other.GetComponent<PlantPickUp>().plant;
        collector.SetPlantData(plant);
        Destroy(other.gameObject);
        isFull = true;
    }
}