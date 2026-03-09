using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public enum ActionMap { Player, UI, Minigame, Menu }

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private List<Action> cancleActions;
    private Action uiSubmitAction;
    
    public void AddOnCancle(Action action) { if (action != null) cancleActions.Add(action); Debug.Log("Cancle lol"); }
    public void AddOnUISubmit(Action action) { if (action != null) uiSubmitAction = action; }

    public void ChangeActionMap(ActionMap map)
    {
        playerInput.SwitchCurrentActionMap(map.ToString());
    }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        cancleActions = new List<Action>();
        ChangeActionMap(ActionMap.Player);
        playerInput.actions.FindActionMap("UI").Disable();
    }

    public void OnCancle(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (cancleActions.Count != 0)
        {
            foreach (var action in cancleActions)
            {
                action.Invoke();
                Debug.Log("ACTION" +  action.ToString());   
            }
            ClearInteract();
        }
       
    }

    public void OnUISubmit(InputAction.CallbackContext context)
    {
        if(!context.canceled) return;
        uiSubmitAction?.Invoke();
    }

    public void ClearInteract()
    {
        cancleActions.Clear();
        uiSubmitAction = null;
    }
}
