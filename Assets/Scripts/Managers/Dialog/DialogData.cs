using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogToken { ACTION, CANCEL }
public enum DialogChoice { CHECK_QUEST, ASK_INFO, CHANGE_QUEST, BACK, NAME, SUB_QUEST };


public class DialogData 
{
    public List<string> keys = new List<string>();
    public List<DialogResponse> responses = new List<DialogResponse>();
}

[System.Serializable]
public struct DialogResponse 
{
    public string responseText;
    public DialogChoice choice;
}

