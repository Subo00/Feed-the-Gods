using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Interactable, DialogUser, IDataPersistence
{
    [SerializeField] private DialogSettings dialogSettings;
    private string characterName;
    
    private DialogManager dialogManager;
    private DialogData[] dialogFirst;
    private DialogData[] dialogSecond;
    private DialogData[] dialogOther;
    
    private QuestManager questManager;
    private Quest[] quests;
    private Quest currentQuest;

    private uint questIndex = 0;
    private bool isFirstTime = true; 
    private bool choicesOpen = false;
    private string prefix = "Offer ";
    private uint otherIndex = 0; //used when traversing dialog menus 

    private float interactionCooldown = 0.5f;
    private float lastInteractionTime = 0f;

    public override void OnInteract(Player player)   
    {
        if (inUse) return;
        interactingPlayer = player;

    }
    protected override void Start()
    {
        dialogManager = DialogManager.Instance;
        questManager = QuestManager.Instance;
        currentQuest = null;
        characterName = gameObject.name;

        base.Start();

        dialogFirst = Resources.LoadAll<DialogData>("Dialog/" + characterName + "/First");
        dialogSecond = Resources.LoadAll<DialogData>("Dialog/" + characterName + "/Second");
        dialogOther = Resources.LoadAll<DialogData>("Dialog/" + characterName + "/Other");

        quests = Resources.LoadAll<Quest>("Quests/" + characterName);
    }

    protected override void OnUpdate()
    {
        //Disables the re-entering in the conversation 
        if (Time.time < lastInteractionTime + interactionCooldown) return;

        if (Input.GetButtonDown("Interact"))
        {

            if (!inUse)
            {
                //Sets self as a current user in dialogManger
                inUse = true;
                dialogManager.ChangeDialogSettings(dialogSettings, characterName);
                dialogManager.SetCurrentUser(this);
                
                //Starts dialog
                if(isFirstTime)
                {
                    isFirstTime = false;
                    dialogManager.StartDialog(dialogFirst[questIndex]);
                    dialogManager.LoadDialog(dialogOther[otherIndex]);
                    currentQuest = Instantiate(quests[questIndex]);
                    currentQuest.onQuestComplete = IncrementIndex;
                    questManager.AddToActive(currentQuest);
                }
                else
                {
                    dialogManager.StartDialog(dialogSecond[questIndex]);
                    dialogManager.LoadDialog(dialogOther[otherIndex]);
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
        questManager.AddToCompleted(quests[questIndex]);
        questIndex++;
        isFirstTime = true;
        return;
    }

    public void OnCheckQuest()
    {
        DialogData checkQuestDialog = new DialogData();
        DialogLine tmpDialogLine = new DialogLine();
        DialogResponse tmpResponse = new DialogResponse();

        currentQuest.CheckProgress();

        if (currentQuest.isCompleted)
        {
            tmpDialogLine.line = "Thank you";
            checkQuestDialog.lines.Add(tmpDialogLine);
            otherIndex = 0;
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
            otherIndex = 1;
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
        if (otherIndex == 0)
        {
            dialogManager.EndDialog();
            inUse = false;
            lastInteractionTime = Time.time;

            return;
        }

        otherIndex = 0;
        dialogManager.LoadDialog(dialogOther[otherIndex]);
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
        //get itemData via name
        item = item.Substring(prefix.Length);

        SubQuest currentSubQuest = new SubQuest();
        foreach(SubQuest subQuest in currentQuest.subQuests)
        {
            if(subQuest.collectData.name == item)
            {
                currentSubQuest = subQuest;
                break;
            }
        }

        //Remove the item from inventory depending on the SubQuest's count
        /*uint currentItemCount = Inventory.Instance.CurrentItemCount(currentSubQuest.collectData);
        uint neededCount = currentSubQuest.requiredValue - currentSubQuest.currentValue;

        if (currentItemCount > 0) 
        {
            if(currentItemCount >= neededCount)
            {
                Inventory.Instance.RemoveItemQuantity(currentSubQuest.collectData, neededCount);
                currentSubQuest.currentValue += neededCount;
            }else
            {
                Inventory.Instance.RemoveItemQuantity(currentSubQuest.collectData, currentItemCount);
                currentSubQuest.currentValue += currentItemCount;
            }
        }*/
        //load OnCheckQuest again to refresh everything
        OnCheckQuest();
    }

    public void LoadData(GameData data)
    {
        /*if (!data.charactersDialog.TryGetValue(name, out questIndex))
        {
           questIndex = 0;
        }
        */
    }

    public void SaveData(ref GameData data)
    {
        /*if (data.charactersDialog.ContainsKey(name))
        {
            data.charactersDialog.Remove(name);
        }
        data.charactersDialog.Add( name, questIndex);
        */
    }
}
