using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHuman : Interactable
{
    [SerializeField] private DialogSettings dialogSettings;
    private string characterName;
    private DialogManager dialogManager;
    [SerializeField] private DialogData dialog;

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
        dialogManager.SetOnEndDialog(Reset);
        OnInteracted();
    }

    private void OnInteracted()
    {
        if (Time.time < lastInteractionTime + interactionCooldown) return;
        //if (interactingPlayer.IsInteracting) return;
        if (!inUse)
        {
            inUse = true;
            dialogManager.ChangeDialogSettings(dialogSettings, characterName);
            dialogManager.StartDialog(dialog);
        }
        else
        {
            dialogManager.DisplayNextDialogueLine();
        }
    }
    protected override void OnUpdate()
    {
        if (Time.time < lastInteractionTime + interactionCooldown) return;

        CommonLogic();
    }

    private void Reset()
    {
        interactingPlayer.SetInteract(OnInteract);
        inUse = false;
        lastInteractionTime = Time.time;
    }
}
