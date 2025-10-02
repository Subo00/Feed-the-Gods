using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameGather :  Minigame
{
    private float progress = 0f;
    [SerializeField] private float step = 0.01f;
    [SerializeField] private ProgressBar progressBar;


    public override void StartMinigame(uint valueUint, MinigameManager manager = null)
    {
        base.StartMinigame(valueUint, manager);
        //PlayerAnimationController.instance.PlayAllLayers(Animations.GATHER);
        //SoundManager.PlaySound(SoundType.Rustle);
    }
    public override void DisruptMinigame() { }
    public override void EndMinigame()
    {
        base.EndMinigame();
    }

    protected override void OnUpdate()
    {
        if(manager.IsInteracting())
        {
            progress += step;
            progressBar.SetFillAmount(progress);

            if (progress >= 1.0f)
            {
                manager.EndMinigame(1f);
                return;
            }
        }
        else
        {
            manager.EndMinigame(-1f);
        }
    }
}
