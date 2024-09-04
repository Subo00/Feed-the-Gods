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
    private QuestManager questManager;
    private bool choicesOpen = false;
    private string prefix = "Offer ";
    private DialogData checkQuestDialog;
    private uint indexOther = 0;
    

    protected override void Start()
    {
        dialogManager = DialogManager.Instance;
        questManager = QuestManager.Instance;
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
        Debug.Log("QUEST COMPLETED"); return;
        questManager.AddToCompleted(quests[index]);
        isFirstTime = true;
        index++;
    }

    public void OnCheckQuest()
    {
        checkQuestDialog = new DialogData();
        DialogLine tmpDialogLine = new DialogLine();
        DialogResponse tmpResponse = new DialogResponse();

        currentQuest.CheckProgress();
        indexOther = 1;

        if (currentQuest.isCompleted)
        {
            tmpDialogLine.line = "Thank you";
            checkQuestDialog.lines.Add(tmpDialogLine);
            //IncrementIndex and load new conversation
        }
        else
        {
            //Create new Dialog from the current index as dialogSecond[i]
            tmpDialogLine.line = "I still need...";
            checkQuestDialog.lines.Add(tmpDialogLine);

            //Modify it for every SubQuest 
            foreach (SubQuest subQuest in currentQuest.subQuests)
            {
                if (!subQuest.IsCompleted())
                {
                    tmpDialogLine.line = subQuest.requiredValue - subQuest.currentValue + " " + subQuest.collectData.name;
                    checkQuestDialog.lines.Add(tmpDialogLine);
                    //Create a response for every subQuest
                    tmpResponse.responseText = prefix + subQuest.collectData.name;
                    tmpResponse.choice = DialogChoice.SubQuest;
                    checkQuestDialog.responses.Add(tmpResponse);
                }
            }
        }
        
        //Add a back responses
        tmpResponse.responseText = "Back";
        tmpResponse.choice = DialogChoice.Back;
        checkQuestDialog.responses.Add(tmpResponse);

        //load the dialog into dialogManager
        dialogManager.LoadDialog(checkQuestDialog);
    }

    public void OnAskInfo()
    {
        //List out the characters and places the player has discovered 
        //And cross that list with the availabel information this character has 
        //Print out the buttons with the names 
        Debug.Log("AskInfo");

    }

    public void OnChangeQuest()
    {
        //List out the completed quests this character offers 
        //Create a button for every quest 
        Debug.Log("ChangeQuest");

    }

    public void OnBack()
    {
        if (indexOther == 0)
        {
            dialogManager.EndDialog();
            return;
        }

        indexOther = 0;
        dialogManager.LoadDialog(dialogOther[indexOther]);
    }

    public void OnName(string name)
    {
        Debug.Log(name);    
    }

    public void DialogActive(bool active)
    {
        choicesOpen = active;
    }

    public void OnSubQuest(string item)
    {
        item = item.Substring(prefix.Length);
        //Remove the item from inventory depending on the SubQuest's requiredCount

        //gets itemData via name
        SubQuest currentSubQuest = new SubQuest();
        foreach(SubQuest subQuest in collector.currentQuest.subQuests)
        {
            if(subQuest.collectData.name == item)
            {
                currentSubQuest = subQuest;
                break;
            }
        }

        int currentItemCount = InventoryManager.Instance.CurrentItemCount(currentSubQuest.collectData);
        int neededCount = currentSubQuest.requiredValue - currentSubQuest.currentValue;

        if (currentItemCount > 0) 
        {
            if(currentItemCount >= neededCount)
            {
                InventoryManager.Instance.RemoveItemQuantity(currentSubQuest.collectData, neededCount);
                currentSubQuest.currentValue += neededCount;
            }else
            {
                InventoryManager.Instance.RemoveItemQuantity(currentSubQuest.collectData, currentItemCount);
                currentSubQuest.currentValue += currentItemCount;
            }
        }
        //load OnCheckQuest again to refresh everything
        OnCheckQuest();
    }
}
