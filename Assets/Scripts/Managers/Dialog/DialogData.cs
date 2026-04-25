using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogToken { ACTION, CANCLE }
public enum DialogChoice { CHECK_QUEST, ASK_INFO, CHANGE_QUEST, BACK, NAME, SUB_QUEST };


[CreateAssetMenu(fileName = "New Item", menuName = "Dialog/Create DialogData")]
public class DialogData : ScriptableObject
{
    public List<string> lines = new List<string>();
    public List<DialogResponse> responses = new List<DialogResponse>();
}

[System.Serializable]
public struct DialogResponse 
{
    public string responseText;
    public DialogChoice choice;
}

