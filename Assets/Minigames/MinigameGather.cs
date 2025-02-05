using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameGather : MonoBehaviour, Minigame
{
    private float progress = 0f;
    [SerializeField] private float step = 0.01f;
    [SerializeField] private ProgressBar progressBar;

    void Minigame.StartMinigame(uint valueUint)
    {
        UpdateManager.Instance.AddUpdatable(this);
        PlayerAnimationController.instance.PlayAllLayers(Animations.GATHER);
        SoundManager.PlaySound(SoundType.Rustle);
    }
    void Minigame.DisruptMinigame() { }
    void Minigame.EndMinigame()
    {
        UpdateManager.Instance.RemoveUpdatable(this);
    }

    void IMyUpdate.MyUpdate()
    {
        if(Input.GetButton("Interact"))
        {
            progress += step;
            progressBar.SetFillAmount(progress);

            if (progress >= 1.0f)
            {
                MinigameManager.Instance.EndMinigame(1f);
                return;
            }
        }
        else
        {
            MinigameManager.Instance.EndMinigame(-1f);
        }
    }
}
