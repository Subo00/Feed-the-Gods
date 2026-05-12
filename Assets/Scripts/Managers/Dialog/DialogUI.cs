using UnityEngine;
using UnityEngine.UI;

public class DialogUI : BaseUI, UIPrompt
{
    [SerializeField] private GameObject confirmButton;
    [SerializeField] private GameObject backButton;
    public void SetPrompt(InputPrompt input)
    {
        switch (input)
        {
            case InputPrompt.WASD:
                confirmButton.GetComponent<Image>().sprite = UIPromptManager.instance.ConfirmWASD;
                backButton.GetComponent<Image>().sprite = UIPromptManager.instance.CancelWASD;
                break;
            case InputPrompt.IJKL:
                confirmButton.GetComponent<Image>().sprite = UIPromptManager.instance.ConfirmIJKL;
                backButton.GetComponent<Image>().sprite = UIPromptManager.instance.CancelIJKL;
                break;
            case InputPrompt.XBOX:
                confirmButton.GetComponent<Image>().sprite = UIPromptManager.instance.ConfirmXBOX;
                backButton.GetComponent<Image>().sprite = UIPromptManager.instance.CancelXBOX;
                break;
            case InputPrompt.PLAYSTATION:
                confirmButton.GetComponent<Image>().sprite = UIPromptManager.instance.ConfirmPS;
                backButton.GetComponent<Image>().sprite = UIPromptManager.instance.CancelPS;
                break;
        }
    }
}
