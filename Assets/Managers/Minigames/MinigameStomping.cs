using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


public class MinigameStomping :  Minigame
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

    public override void StartMinigame(uint valueUint, MinigameManager manager = null)
    {
        base.StartMinigame(valueUint, manager);
        numToSpawn = valueUint;

        endGoal = calculateFloat(valueUint);
        threshold = calculateFloat(counter);
        Debug.Log("endGoal: " + endGoal);

        PlayerAnimationController.instance.PlayAllLayers(Animations.IDLE);
        PlayerAnimationController.instance.Play(Animations.STOMP, 2, false, false);
    }

    public override void EndMinigame()
    {
        base.EndMinigame();
        PlayerAnimationController.instance.PlayAllLayers(Animations.NONE);
    }

    protected override void OnUpdate()
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
            manager.EndMinigame(-1f);
        }
        

        progressBar.SetFillAmount(progress/endGoal);

        if (progress >= endGoal)
        {
            manager.EndMinigame((float)numToSpawn);
        }
        else if(progress >= threshold)
        {
            manager.SpawnFromSpawner();
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
