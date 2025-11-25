using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameGather :  Minigame
{
    private float progress = 0f;
    [SerializeField] private float step = 0.01f;
    [SerializeField] private ProgressBar progressBar;


    public override void StartMinigame(uint valueUint, MinigameManager minigameManager = null)
    {
        base.StartMinigame(valueUint, minigameManager);
        manager.Player.Animator.PlayAllLayers(Animations.GATHER);
        manager.Player.ToggleUI(true);
        SoundManager.PlaySound(SoundType.Rustle);
    }
    public override void DisruptMinigame() { }
    public override void EndMinigame()
    {
        base.EndMinigame();
        manager.Player.Animator.PlayAllLayers(Animations.IDLE);
        manager.Player.ToggleUI(false);
    }

    protected override void OnUpdate()
    {
        if(manager.Player.IsInteracting)
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
