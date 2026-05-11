using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputPromptExtension
{
    private const string WasdInteractLabel = "E";
    private const string IjklInteractLabel = "U";
    private const string XboxInteractLabel = "A";
    private const string PsInteractLabel = "X";

    private const string WasdCancelLabel = "Q";
    private const string IjklCancelLabel = "U";
    private const string XboxCancelLabel = "B";
    private const string PsCancelLabel = "\u25CB";

    public static string GetInteractLabel(this InputPrompt prompt)
    {
        return prompt switch
        {
            InputPrompt.WASD => WasdInteractLabel,
            InputPrompt.IJKL => IjklInteractLabel,
            InputPrompt.XBOX => XboxInteractLabel,
            InputPrompt.PLAYSTATION => PsInteractLabel,
            _ => "?"
        };
    }

    public static string GetCancleLabel(this InputPrompt prompt)
    {
        return prompt switch
        {
            InputPrompt.WASD => WasdCancelLabel,
            InputPrompt.IJKL => IjklCancelLabel,
            InputPrompt.XBOX => XboxCancelLabel,
            InputPrompt.PLAYSTATION => PsCancelLabel,
            _ => "?"
        };
    }
}