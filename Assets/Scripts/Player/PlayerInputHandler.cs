using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public enum ActionMap { Player, UI, Minigame, Menu }

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private List<Action> cancelActions;
    private Action uiSubmitAction;
    private Action uiCancelAction;

    public void AddOnCancel(Action action) { if (action != null) cancelActions.Add(action);  }
    public void AddOnUISubmit(Action action) { if (action != null) uiSubmitAction = action; }
    public void AddOnUICancel(Action action) { if (action != null) uiCancelAction = action; }   

    public void ChangeActionMap(ActionMap map)
    {
        playerInput.SwitchCurrentActionMap(map.ToString());
    }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        cancelActions = new List<Action>();
        ChangeActionMap(ActionMap.Player);
        playerInput.actions.FindActionMap("UI").Disable();
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (cancelActions.Count != 0)
        {
            foreach (var action in cancelActions)
            {
                action.Invoke();
            }
            ClearInteract();
        }
        else
        {
            uiCancelAction?.Invoke();
        }
       
    }

    public void OnUISubmit(InputAction.CallbackContext context)
    {
        if(!context.canceled) return;
        uiSubmitAction?.Invoke();
    }

    public void ClearInteract()
    {
        cancelActions.Clear();
        uiSubmitAction = null;
    }
}
