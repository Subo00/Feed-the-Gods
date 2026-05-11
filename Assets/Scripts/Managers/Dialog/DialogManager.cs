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
    private Queue<string> lines;
    private PlayerUIManager uiManager;
    private bool endOfLine = true;
    private string currentLine;
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
        lines = new Queue<string>();
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

    public void StartDialog(string key)
    {
        if (LocalizationManager.Instance == null)
        {
            Debug.LogError("[DialogManager] LocalizationManager not found.");
            return;
        }

        List<string> resolvedLines = LocalizationManager.Instance.GetLines(key);

        if (resolvedLines.Count == 0)
        {
            Debug.LogWarning($"[DialogManager] No lines found for key prefix: {key}");
            return;
        }

        uiManager.ToggleDialog();

        Clear();

        foreach (string text in resolvedLines)
            lines.Enqueue(text);

        DisplayNextDialogueLine();
    }

    public void LoadDialog(DialogData dialog)
    {
        LoadLines(dialog.keys);
        LoadResponses(dialog.responses);
    }
    public void LoadLines(List<string> keys)
    {
        foreach (string key in keys)
        {
            if (string.IsNullOrEmpty(key) || LocalizationManager.Instance == null) return;

            List<string> resolvedLines = LocalizationManager.Instance.GetLines(key);

            if (resolvedLines.Count > 0)
            {
                foreach (string text in resolvedLines)
                    lines.Enqueue(text);
            }else
            {
                lines.Enqueue(key);
            }
        }
        DisplayNextDialogueLine();
    }

    public void LoadResponses(List<DialogResponse> dialogResponses)
    {
        foreach (DialogResponse dialogResponse in dialogResponses)
        {
            responses.Enqueue(dialogResponse);
        }
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
            currentLine = ReplaceTokens(currentLine);

            StopAllCoroutines();
            StartCoroutine(TypeSentence());
            endOfLine = false;
        }
        else
        {
            StopAllCoroutines();
            dialogArea.text = currentLine;
            endOfLine = true;
        }
        
    }

    IEnumerator TypeSentence()
    {
        dialogArea.text = "";
        string line = currentLine;

        foreach (char letter in line)
        {
            dialogArea.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        endOfLine = true;
    }

    
    private string ReplaceTokens(string line)
    {
        System.Text.StringBuilder result = new System.Text.StringBuilder();
        int i = 0;

        while (i < line.Length)
        {
            if (line[i] == '[')
            {
                int close = line.IndexOf(']', i);
                if (close > i)
                {
                    string token = line.Substring(i + 1, close - i - 1);
                    result.Append(ResolveToken(token));
                    i = close + 1;
                    continue;
                }
            }

            result.Append(line[i]);
            i++;
        }

        return result.ToString();
    }

    private string ResolveToken(string token)
    {
        if (System.Enum.TryParse(token, ignoreCase: true, out DialogToken dialogToken))
        {
            return dialogToken switch
            {
                DialogToken.ACTION => prompt.GetInteractLabel(),
                DialogToken.CANCEL => prompt.GetCancleLabel(),
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
            case DialogChoice.CHECK_QUEST:
                currentUser.OnCheckQuest(); break;
            case DialogChoice.ASK_INFO:
                currentUser.OnAskInfo(); break;
            case DialogChoice.BACK:
                currentUser.OnBack(); break;
            case DialogChoice.CHANGE_QUEST:
                currentUser.OnChangeQuest(); break;
            case DialogChoice.NAME:
                currentUser.OnName(response.responseText); break;
            case DialogChoice.SUB_QUEST:
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
