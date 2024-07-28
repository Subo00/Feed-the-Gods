using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Interactable
{
    [SerializeField] private DialogSettings dialogSettings;
    [SerializeField] private Dialog[] dialogFirst;
    [SerializeField] private Dialog[] dialogOther;
    [SerializeField] private string name;
    [SerializeField] private Quest[] quests;
    
    private DialogManager dialogManager;
    private uint index = 0;
    private bool isFirstTime = true;
    private CharacterCollector collector;
    private QuestManager questManager;

    protected override void Start()
    {
        dialogManager = DialogManager.Instance;
        questManager = QuestManager.Instance;

        collector = gameObject.GetComponentInChildren<CharacterCollector>();
        if(collector == null)
        {
            Debug.LogError("Attach CharacterCollector as a child of the " + name); 
        }

        foreach(Quest quest in quests)
        {
            quest.onQuestComplete = IncrementIndex;
        }

        base.Start();
    }

    protected override void OnUpdate()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (!inUse)
            {
                inUse = true;
                dialogManager.ChangeDialogSettings(dialogSettings, name);
                if(isFirstTime)
                {
                    isFirstTime = false;
                    dialogManager.StartDialog(dialogFirst[index]);
                    questManager.AddToActive(quests[index]);
                    collector.SetCurrentQuest(quests[index]);
                }
                else
                {
                    dialogManager.StartDialog(dialogOther[index]);  
                }
            }
            else
            {
                dialogManager.DisplayNextDialogueLine();
            }
        }
        else
        {
            CommonLogic();
        }
    }

    public void IncrementIndex()
    {
        questManager.AddToCompleted(quests[index]);
        isFirstTime = true;
        index++;
    }
}
