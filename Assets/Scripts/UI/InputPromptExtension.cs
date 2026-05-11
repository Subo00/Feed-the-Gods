using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputPromptExtension
{
    private const string WasdInteractLabel = "E";
    private const string IjklInteractLabel = "U";
    private const string XboxInteractLabel = "A";
    private const string PsInteractLabel = "X";

    private const string WasdCancleLabel = "Q";
    private const string IjklCancleLabel = "U";
    private const string XboxCancleLabel = "B";
    private const string PsCancleLabel = "\u25CB";

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
            InputPrompt.WASD => WasdCancleLabel,
            InputPrompt.IJKL => IjklCancleLabel,
            InputPrompt.XBOX => XboxCancleLabel,
            InputPrompt.PLAYSTATION => PsCancleLabel,
            _ => "?"
        };
    }
}