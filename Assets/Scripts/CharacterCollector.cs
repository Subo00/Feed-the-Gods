using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollector : MonoBehaviour
{
    public Quest currentQuest = null;

    public void SetCurrentQuest(Quest currentQuest)
    {
        this.currentQuest = currentQuest;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (currentQuest == null || other.GetComponent<ItemPickUp>() == null)
        {
            return;
        }

        foreach (SubQuest subQuest in currentQuest.subQuests)
        {
            if (other.GetComponent<ItemPickUp>().item == subQuest.collectData && !subQuest.IsCompleted())
            {
                subQuest.IncrementValue();
                Destroy(other.gameObject);
            }
        }
        currentQuest.CheckProgress();
    }
}
