using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct DialogLine
{
    [TextArea(3, 10)]
    public string line;
}

[CreateAssetMenu(fileName = "New Item", menuName = "Dialog/Create DialogData")]
public class DialogData : ScriptableObject
{
    public List<DialogLine> lines = new List<DialogLine>();
    public List<DialogResponse> responses = new List<DialogResponse>();
}

public enum DialogChoice { CheckQuest, AskInfo, ChangeQuest, Back, Name, SubQuest };
[System.Serializable]
public struct DialogResponse 
{
    public string responseText;
    public DialogChoice choice;
}

