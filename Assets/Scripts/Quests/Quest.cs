using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SubQuest
{
    //collectData  can be anything you need it to be
    public ItemData collectData;
    public uint requiredValue;
    public uint currentValue = 0;

    public void IncrementValue()
    {
        currentValue++;
    }
    public bool IsCompleted()
    {
        return currentValue == requiredValue;
    }
}

[CreateAssetMenu(fileName = "New Item", menuName = "Quest/Create Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    public string questGiver;
    public string description;
    public SubQuest[] subQuests;

    public event System.Action<Quest> onQuestChanged;
    public System.Action onQuestComplete;

    public bool isCompleted = false;

    public void CheckProgress()
    {
        if (isCompleted) return;
        int completedSubQuests = 0;
        foreach (var subQuest in subQuests)
        {
            if (subQuest.IsCompleted())
            {
                completedSubQuests++;
            }
        }
        onQuestChanged?.Invoke(this);

        if (completedSubQuests == subQuests.Length)
        {
            isCompleted = true;
            onQuestComplete?.Invoke();
        }
    }

}
