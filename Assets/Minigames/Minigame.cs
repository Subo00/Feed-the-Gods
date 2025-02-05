using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Minigame : IMyUpdate
{
    void StartMinigame(uint uintValue = 0);
    void EndMinigame();
    void DisruptMinigame();
}
