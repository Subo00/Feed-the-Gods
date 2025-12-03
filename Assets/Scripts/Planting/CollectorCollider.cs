using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorCollider : MonoBehaviour
{
    private List<ItemStack> neededItems = new List<ItemStack>();
    private Collector collector = null;

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
        if (!other.GetComponent<ItemPickUp>())
        {
            return;
        }
        ItemData item = other.GetComponent<ItemPickUp>().item;
        if (neededItems.Count == 0 || item == null)
        {
            return;
        }

        foreach (ItemStack itemStack in neededItems)
        {
            if (item == itemStack.getItemData())
            {
                if (itemStack.RemoveFromStack() == true)
                {
                    //if the stack is at 0 remove from list
                    neededItems.Remove(itemStack);
                }
                collector.ReportBool(neededItems.Count == 0);
                //SoundManager.PlaySound(SoundType.Absorb);
                Destroy(other.gameObject);
                break;
            }
        }
    }

    public void SetNeededItems(List<ItemStack> items)
    {
        neededItems.Clear();
        neededItems = new List<ItemStack>(items);
    }

}