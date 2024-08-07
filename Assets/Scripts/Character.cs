using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Interactable, DialogUser
{
    [SerializeField] private DialogSettings dialogSettings;
    [SerializeField] private DialogData[] dialogFirst;
    [SerializeField] private DialogData[] dialogSecond;
    [SerializeField] private DialogData[] dialogOther;
    [SerializeField] private string name;
    [SerializeField] private Quest[] quests;
    
    private DialogManager dialogManager;
    private uint index = 0;
    private bool isFirstTime = true;
    private CharacterCollector collector;
    private QuestManager questManager;
    private bool choicesOpen = false;
    private string prefix = "Offer ";
    private DialogData checkQuestDialog;
    private uint indexOther = 0;
    

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

        dialogFirst = Resources.LoadAll<DialogData>("Dialog/" + name + "/First");
        dialogSecond = Resources.LoadAll<DialogData>("Dialog/" + name + "/Second");
        dialogOther = Resources.LoadAll<DialogData>("Dialog/" + name + "/Other");
        checkQuestDialog = new DialogData();
    }

    protected override void OnUpdate()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (!inUse)
            {
                //Sets self as a current user in dialogManger
                inUse = true;
                dialogManager.ChangeDialogSettings(dialogSettings, name);
                dialogManager.SetCurrentUser(this);
                
                //Starts dialog
                if(isFirstTime)
                {
                    isFirstTime = false;
                    dialogManager.StartDialog(dialogFirst[index]);
                    dialogManager.LoadDialog(dialogOther[indexOther]);
                    questManager.AddToActive(quests[index]);
                    collector.SetCurrentQuest(quests[index]);
                }
                else
                {
                    dialogManager.StartDialog(dialogSecond[index]);
                    dialogManager.LoadDialog(dialogOther[indexOther]);
                }
            }
            else
            {
                if (!choicesOpen) dialogManager.DisplayNextDialogueLine();
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
