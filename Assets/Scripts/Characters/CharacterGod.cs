using UnityEngine;

public class CharacterGod : Interactable, DialogUser, IDataPersistence
{
    [SerializeField] private DialogSettings dialogSettings;
    [SerializeField] private string[] dialogKeys;
    [SerializeField] private DialogData dialogMenu;
   
    private string characterName;
    private DialogManager dialogManager;
    private QuestManager questManager;
    private Quest[] quests;
    private Quest currentQuest;

    private uint questIndex = 0;
    private bool isFirstTime = true;
    private bool choicesOpen = false;
    private string prefix = "Offer ";
    private string thanks = "Thank you";
    private string need = "Need";
    private string back = "BBBBB";
    private uint otherIndex = 0; //used when traversing dialog menus 

    private float interactionCooldown = 0.5f;
    private float lastInteractionTime = 0f;

    public override void OnInteract(Player player)
    {
        if (inUse) return;
        interactingPlayer = player;
        dialogManager = interactingPlayer.DialogManager;
        interactingPlayer.ClearInteract();
        interactingPlayer.InputHandler.AddOnUISubmit(OnInteracted);
        interactingPlayer.InputHandler.ChangeActionMap(ActionMap.UI);
        OnInteracted();
    }

    private void OnInteracted()
    {
        if (Time.time < lastInteractionTime + interactionCooldown) return;
        //if (interactingPlayer.IsInteracting) return;
        if (!inUse)
        {
            SetUpTalking();
        }
        else
        {
            if (!choicesOpen) dialogManager.DisplayNextDialogueLine();
        }
    }
    protected override void Start()
    {
        questManager = QuestManager.Instance;
        currentQuest = null;
        characterName = gameObject.name;

        base.Start();
        SetUpResponses();

        quests = Resources.LoadAll<Quest>("Quests/" + characterName);
    }

    protected override void OnUpdate()
    {
        //Disables the re-entering in the conversation 
        if (Time.time < lastInteractionTime + interactionCooldown) return;

        CommonLogic();
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
            tmpDialogLine.line = thanks; 
            checkQuestDialog.lines.Add(tmpDialogLine);
            otherIndex = 0;
        }
        else
        {
            //Create new Dialog from the current index as dialogSecond[i]
            tmpDialogLine.line = need; 
            checkQuestDialog.lines.Add(tmpDialogLine);

            //Modify it for every SubQuest 
            foreach (SubQuest subQuest in currentQuest.subQuests)
            {
                if (!subQuest.IsCompleted())
                {
                    tmpDialogLine.line = subQuest.requiredValue - subQuest.currentValue + " " + subQuest.collectData.name;
                    checkQuestDialog.lines.Add(tmpDialogLine);
                    //Create a response for every subQuest
                    tmpResponse.responseText = prefix + " " + subQuest.collectData.name;
                    tmpResponse.choice = DialogChoice.SUB_QUEST;
                    checkQuestDialog.responses.Add(tmpResponse);
                }
            }
            otherIndex = 1;
        }

        //Add a back responses
        tmpResponse.responseText = back;
        tmpResponse.choice = DialogChoice.BACK;
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
            interactingPlayer.InputHandler.ClearInteract();
            dialogManager.EndDialog();
            interactingPlayer.SetInteract(OnInteract);
            inUse = false;
            lastInteractionTime = Time.time;

            return;
        }

        otherIndex = 0;
        dialogManager.LoadDialog(dialogMenu);
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
        foreach (SubQuest subQuest in currentQuest.subQuests)
        {
            if (subQuest.collectData.name == item)
            {
                currentSubQuest = subQuest;
                break;
            }
        }

        //Remove the item from inventory depending on the SubQuest's count
        uint currentItemCount = interactingPlayer.Inventory.CurrentItemCount(currentSubQuest.collectData);
        uint neededCount = currentSubQuest.requiredValue - currentSubQuest.currentValue;

        if (currentItemCount > 0)
        {
            if (currentItemCount >= neededCount)
            {
                interactingPlayer.Inventory.RemoveItemQuantity(currentSubQuest.collectData, neededCount);
                currentSubQuest.currentValue += neededCount;
            }
            else
            {
                interactingPlayer.Inventory.RemoveItemQuantity(currentSubQuest.collectData, currentItemCount);
                currentSubQuest.currentValue += currentItemCount;
            }
        }
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

    private void SetUpTalking()
    {
        //Sets self as a current user in dialogManger
        inUse = true;
        dialogManager.ChangeDialogSettings(dialogSettings, characterName);
        dialogManager.SetCurrentUser(this);

        string fullKey = characterName.ToUpper() + "_" + dialogKeys[questIndex];

        //Starts dialog
        if (isFirstTime)
        {
            isFirstTime = false;
            dialogManager.StartDialog(fullKey);
            //dialogManager.LoadDialog(dialogKeyOther[otherIndex]);
            dialogManager.LoadDialog(dialogMenu);
            currentQuest = Instantiate(quests[questIndex]);
            currentQuest.onQuestComplete = IncrementIndex;
            questManager.AddToActive(currentQuest);
        }
        else
        {
            dialogManager.StartDialog(fullKey + "_SECOND");

            dialogManager.LoadDialog(dialogMenu);
        }
    }

    private void SetUpResponses()
    {
        LocalizationManager locManager = LocalizationManager.Instance;

        prefix = locManager.GetLine("RESPONSE_OFFER");
        thanks = locManager.GetLine("RESPONSE_THANKS");
        need = locManager.GetLine("RESPONSE_NEED");
        back = locManager.GetLine("RESPONSE_BACK");
    }
}
