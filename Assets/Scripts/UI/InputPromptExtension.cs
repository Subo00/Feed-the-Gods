using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputPromptExtension
{
    private const string WasdInteractLabel = "E";
    private const string IjklInteractLabel = "U";
    private const string XboxInteractLabel = "A";

    private const string WasdCancleLabel = "Q";
    private const string IjklCancleLabel = "U";
    private const string XboxCancleLabel = "B";

    public static string GetInteractLabel(this InputPrompt prompt)
    {
        return prompt switch
        {
            InputPrompt.WASD => WasdInteractLabel,
            InputPrompt.IJKL => IjklInteractLabel,
            InputPrompt.XBOX => XboxInteractLabel,
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
            _ => "?"
        };
    }
}