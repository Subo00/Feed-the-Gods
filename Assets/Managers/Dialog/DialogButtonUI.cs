using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogButtonUI : ButtonUI
{
    private DialogResponse response;
    [SerializeField] private TMP_Text itemName;
 

    public void Initialize(DialogResponse response, System.Action<DialogResponse> onClick/*, System.Action<RectTransform> onButtonSelect*/)
    {
        // Change the look
        this.response = response;
        itemName.text = response.responseText;

        // Add functionality to the button 
        button.onClick.AddListener(() => { onClick(this.response); });

        /*// Add on button select listener
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>(); // Add EventTrigger component to button
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Select; // Set event type to OnSelect
        entry.callback.AddListener((data) => { onButtonSelect(button.GetComponent<RectTransform>()); });
        trigger.triggers.Add(entry);*/
    }
}
