using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUI : BaseUI
{
    public bool questEmpty = true;
    public static QuestUI Instance;

    //TODO: [SerializeField] private GameObject progressBarPrefab;
    [SerializeField] private ProgressBar progressBar;
    [SerializeField] private TMP_Text tmpText;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    private void Start()
    {
        //TODO: load from saved point the quests 
        //SetCurrentQuest(null);
    }

    public void SetCurrentQuest(Quest quest)
    {
        if(quest == null)
        {
            Toggle(false);
            return;
        }

        //set the name of the quest in the tmptext
        tmpText.text = quest.questName;
        //TODO Create ProgressBars depending on how much subQuests the quest has

        //progressBar.gameObject.SetActive(true);
        progressBar.SetFillAmount((int)quest.subQuests[0].currentValue, (int)quest.subQuests[0].requiredValue);
        //update each of the ProgressBars acordingly 
    }

 
}
