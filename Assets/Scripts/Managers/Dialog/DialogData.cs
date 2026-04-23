using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogToken { ACTION, CANCLE }
public enum DialogChoice { CHECK_QUEST, ASK_INFO, CHANGE_QUEST, BACK, NAME, SUB_QUEST };

[System.Serializable]
public struct DialogLine
{
    public string key;

    [TextArea(3, 10)]
    public string line;
}

[CreateAssetMenu(fileName = "New Item", menuName = "Dialog/Create DialogData")]
public class DialogData : ScriptableObject
{
    public List<DialogLine> lines = new List<DialogLine>();
    public List<DialogResponse> responses = new List<DialogResponse>();
}

[System.Serializable]
public struct DialogResponse 
{
    public string responseText;
    public DialogChoice choice;
}

