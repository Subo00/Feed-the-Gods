using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct DialogSettings
{
    public Sprite imageSprite;
    public Font font;
    public float textSpeed;
}



public interface DialogUser 
{
    void OnCheckQuest();
    void OnAskInfo();
    void OnChangeQuest();
    void OnBack();
    void OnName(string name);
    void OnSubQuest(string item);
    void DialogActive(bool active);
}
