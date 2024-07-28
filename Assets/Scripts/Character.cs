using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Interactable
{
    [SerializeField] private DialogSettings dialogSettings;
    [SerializeField] private Dialog[] dialogFirst;
    [SerializeField] private Dialog[] dialogOther;
    private DialogManager dialogManager;
    private uint index = 0;
    private bool isFirstTime = true;
    private CharacterCollector collector;

    protected override void Start()
    {
        dialogManager = DialogManager.Instance;
        collector = gameObject.GetComponentInChildren<CharacterCollector>();
        if(collector == null)
        {
            Debug.LogError("Attach CharacterCollector as a child of the " + name); 
        }

        base.Start();
    }

    protected override void OnUpdate()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (!inUse)
            {
                inUse = true;
                dialogManager.ChangeDialogSettings(dialogSettings);
                if(isFirstTime)
                {
                    isFirstTime = false;
                    dialogManager.StartDialog(dialogFirst[index]);
                }
                else
                {
                    dialogManager.StartDialog(dialogOther[index]);  
                }
            }
            else
            {
                dialogManager.DisplayNextDialogueLine();
            }
        }
        else
        {
            CommonLogic();
        }
    }
}
