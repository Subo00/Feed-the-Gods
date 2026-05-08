using UnityEngine;
using UnityEngine.UI;

public class DialogUI : BaseUI, UIPrompt
{
    [SerializeField] private GameObject confirmButton;
    public void SetPrompt(InputPrompt input)
    {
        switch (input)
        {
            case InputPrompt.WASD:
                confirmButton.GetComponent<Image>().sprite = UIPromptManager.instance.ConfirmWASD;
                break;
            case InputPrompt.IJKL:
                confirmButton.GetComponent<Image>().sprite = UIPromptManager.instance.ConfirmIJKL;
                break;
            case InputPrompt.XBOX:
                confirmButton.GetComponent<Image>().sprite = UIPromptManager.instance.ConfirmXBOX;
                break;
        }
    }
}
