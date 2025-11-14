using System.Collections.Generic;
using UnityEngine;



public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    private List<Quest> activeQuests  = new List<Quest>();
    private List<Quest> completedQuests = new List<Quest>();
    private Quest currentQuest;

    private QuestUI questUI;

    private void Awake()
    {
        if (Instance != null)
        {
            this.gameObject.SetActive(false);
            return;
        }
        Instance = this;

        //TODO: load form data current quest

        currentQuest = null;
    }

    private void Start()
    {
        questUI = QuestUI.Instance;
    }

    public void AddToActive(Quest quest)
    {
        activeQuests.Add(quest);
        quest.onQuestChanged += HandleQuestProgressChanged;
        if (currentQuest == null)
        {
           SetCurrentQuest(quest);
        }
    }

    public void AddToCompleted(Quest quest) 
    {
        if(quest.reward != Reward.NONE) { HandleReward(quest.reward, quest.rewardID); }
            
        activeQuests.Remove(quest);
        completedQuests.Add(quest);
        quest.onQuestChanged -= HandleQuestProgressChanged;
        currentQuest = null;
    } 

    public void SetCurrentQuest(Quest quest)
    {
        currentQuest = quest;
        questUI.SetCurrentQuest(currentQuest);
    }

    private void HandleQuestProgressChanged(Quest quest)
    {
        if(quest.name == currentQuest.name)
        {
            questUI.SetCurrentQuest(quest);
        }
    }

    private void HandleReward(Reward reward, int id)
    {
        switch(reward)
        {
            case Reward.RECIPE:
                ProgressManager.Instance.AddToDiscoveredRecipes(id);
                return;
        }
    }
}
