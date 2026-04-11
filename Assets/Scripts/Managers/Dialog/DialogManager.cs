using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class DialogManager : MonoBehaviour
{
    
    //Variables for text
    [SerializeField] private Image characterImage;
    [SerializeField] private Font font; //add changing of the font
    [SerializeField] private float textSpeed;
    [SerializeField] private TextMeshProUGUI nameArea;
    [SerializeField] private TextMeshProUGUI dialogArea;
    
    //Variables for buttons
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private RectTransform panelTransform;
    [SerializeField] private GameObject responsePanel;
    [SerializeField] private ScrollRect scrollRect;

    public Queue<DialogResponse> responses;

    private InputPrompt prompt;
    private Queue<DialogLine> lines;
    private PlayerUIManager uiManager;
    private bool endOfLine = true;
    private DialogLine currentLine;
    private DialogUser currentUser;
    private int constraintsCount = 4;
    private System.Action onEndDialog;
    public void SetPlayerUIManager(PlayerUIManager playerUI)
    {
        uiManager = playerUI;
    }

    public void SetPrompt(InputPrompt prompt) { this.prompt = prompt; }

    public void SetOnEndDialog(System.Action callback) => onEndDialog = callback;

    private void Start()
    {
        lines = new Queue<DialogLine>();
        responses = new Queue<DialogResponse>();
        constraintsCount = panelTransform.GetComponent<GridLayoutGroup>().constraintCount;
    }

    public void ChangeDialogSettings(DialogSettings settings, string characterName)
    {
        characterImage.sprite = settings.imageSprite;
        font = settings.font;
        textSpeed = settings.textSpeed;
        nameArea.text = characterName;
    }

    public void SetCurrentUser(DialogUser user) { this.currentUser = user; }
    public void StartDialog(DialogData dialog)
    {
        uiManager.ToggleDialog();

        Clear();

        LoadDialog(dialog);
    }

    public void LoadDialog(DialogData dialog)
    {
        foreach (DialogLine dialogLine in dialog.lines)
        {
            lines.Enqueue(dialogLine);
        }

        foreach(DialogResponse dialogResponse in dialog.responses)
        {
            responses.Enqueue(dialogResponse);
        }

        DisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0 && endOfLine)
        {
            EndDialog();
            return;
        }

        if(endOfLine)
        {
            currentLine = lines.Dequeue();

            StopAllCoroutines();
            StartCoroutine(TypeSentence());
            endOfLine = false;
        }
        else
        {
            StopAllCoroutines();
            dialogArea.text = currentLine.line;
            endOfLine = true;
        }
        
    }

    IEnumerator TypeSentence()
    {
        dialogArea.text = "";
        string line = currentLine.line;
        int i = 0;

        while (i < line.Length)
        {
            char letter = line[i];

            if (letter == '[')
            {
                int close = line.IndexOf(']', i);
                if (close > i)
                {
                    string token = line.Substring(i + 1, close - i - 1);
                    dialogArea.text += ResolveToken(token);
                    i = close + 1;
                    continue;
                }
            }

            dialogArea.text += letter;
            yield return new WaitForSeconds(textSpeed);
            i++;
        }

        endOfLine = true;
    }

    private string ResolveToken(string token)
    {
        if (System.Enum.TryParse(token, ignoreCase: true, out DialogToken dialogToken))
        {
            return dialogToken switch
            {
                DialogToken.ACTION => prompt.GetInteractLabel(),
                DialogToken.CANCLE => prompt.GetCancleLabel(),
                _ => $"[{token}]"
            };
        }
        return $"[{token}]";
    }

    public void EndDialog()
    {
        //if dialog doesn't have any response
        if (responses.Count == 0 || responses == null)
        {
            uiManager.ToggleDialog();
            onEndDialog?.Invoke();
            onEndDialog = null;
        }
        else
        {
            DisplayResponses();            
        }
    }

    private void DisplayResponses()
    {
        responsePanel.SetActive(true);

        List<GameObject> dialogButtons = new List<GameObject>();
        
        //Crete buttons
        foreach (DialogResponse response in responses)
        {
            GameObject buttonInstance = Instantiate(buttonPrefab, panelTransform);
            buttonInstance.GetComponent<DialogButtonUI>().Initialize(response, OnResponseClick/*, OnResponseSelect*/);
            dialogButtons.Add(buttonInstance);
        }

        ManagerHelper.ConnectButtons(dialogButtons, constraintsCount);
       
        LayoutRebuilder.ForceRebuildLayoutImmediate(panelTransform.GetComponent<RectTransform>());
        uiManager.ShowDialogResponse(true);
        currentUser.DialogActive(true);
    }

    private void OnResponseClick(DialogResponse response)
    {
        Clear();
        responsePanel.SetActive(false);
        uiManager.ShowDialogResponse(false);
        currentUser.DialogActive(false);


        switch (response.choice) {
            case DialogChoice.CheckQuest:
                currentUser.OnCheckQuest(); break;
            case DialogChoice.AskInfo:
                currentUser.OnAskInfo(); break;
            case DialogChoice.Back:
                currentUser.OnBack(); break;
            case DialogChoice.ChangeQuest:
                currentUser.OnChangeQuest(); break;
            case DialogChoice.Name:
                currentUser.OnName(response.responseText); break;
            case DialogChoice.SubQuest:
                currentUser.OnSubQuest(response.responseText); break;
        }
    }

    /*private void OnResponseSelect(RectTransform selectedButton)
    {
        // Get the position of the selected button in the ScrollRect's content area
        Vector3 selectedButtonPosition = selectedButton.localPosition;

        // Get the height of the content (assuming vertical scrolling)
        float contentHeight = panelTransform.rect.height;

        // Calculate the vertical scroll position (0 is at the bottom, 1 is at the top)
        float normalizedPosition = Mathf.Clamp01(1 - (selectedButtonPosition.y / contentHeight));

        // Set the ScrollRect to that normalized position (0 is bottom, 1 is top)
        scrollRect.verticalNormalizedPosition = normalizedPosition;
    }*/

    private void Clear()
    {
        foreach (Transform child in panelTransform)
        {
            GameObject.Destroy(child.gameObject);
        }
        responses.Clear();
        lines.Clear();

    }
}
