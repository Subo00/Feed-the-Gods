using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


public class MinigameStomping : MonoBehaviour, Minigame
{
    [SerializeField] private float step = 0.15f;
    [SerializeField] private ProgressBar progressBar;
    [SerializeField] private FlickeringImage left;
    [SerializeField] private FlickeringImage right;

    private float progress = 0f;
    private float endGoal = 1f;
    private uint numToSpawn = 1;
    private bool rightLeg = false;
    private uint counter = 1;
    private float threshold = 0f;

    void Minigame.StartMinigame(uint valueUint) 
    {
        numToSpawn = valueUint;

        endGoal = calculateFloat(valueUint);
        threshold = calculateFloat(counter);
        Debug.Log("endGoal: " + endGoal);

        UpdateManager.Instance.AddUpdatable(this);
        PlayerAnimationController.instance.PlayAllLayers(Animations.IDLE);
        PlayerAnimationController.instance.Play(Animations.STOMP, 2, false, false);
    }
    void Minigame.DisruptMinigame() { }
    void Minigame.EndMinigame() 
    {
        UpdateManager.Instance.RemoveUpdatable(this);
        PlayerAnimationController.instance.PlayAllLayers(Animations.NONE);
    }

    void IMyUpdate.MyUpdate() 
    {
        // Handle player input and update the progress bar
        if (Input.GetAxisRaw("Horizontal") == -1f && rightLeg)
        {
            progress += step;
            rightLeg = !rightLeg;
        }
        if (Input.GetAxisRaw("Horizontal") == 1f && !rightLeg)
        {
            progress += step;
            rightLeg = !rightLeg;
        }

        if (Input.GetButtonDown("Cancel"))
        {
            MinigameManager.Instance.EndMinigame(-1f);
        }
        

        progressBar.SetFillAmount(progress/endGoal);

        if (progress >= endGoal)
        {
            MinigameManager.Instance.EndMinigame((float)numToSpawn);
        }
        else if(progress >= threshold)
        {
            MinigameManager.Instance.SpawnFromSpawner();
            counter++;
            threshold = calculateFloat(counter);
        }

        //blinks the buttons on screen
        left.CallUpdate();
        right.CallUpdate();
    }

    private float calculateFloat(uint value)
    {
        return (value * (1 - value / 20f));
    }
}
