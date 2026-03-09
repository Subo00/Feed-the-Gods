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

    public override void StartMinigame(uint valueUint, MinigameManager minigameManager = null)
    {
        base.StartMinigame(valueUint, minigameManager);
        numToSpawn = valueUint;

        endGoal = calculateFloat(valueUint);
        threshold = calculateFloat(counter);
        //Debug.Log("endGoal: " + endGoal);

        manager.Player.InputHandler.ChangeActionMap(ActionMap.Minigame);
        manager.SetOnLeft(MoveLeft);
        manager.SetOnRight(MoveRight);

        //adding a cancle option to the minigame only fuck shti up even more
        //manager.Player.InputHandler.AddOnCancle(() => manager.EndMinigame(-1f) );
        manager.SetCancle(() => manager.EndMinigame(-1f));
        manager.Player.Animator.PlayAllLayers(Animations.IDLE);
        manager.Player.Animator.Play(Animations.STOMP, 2, false, false);
    }

    public override void EndMinigame()
    {
        base.EndMinigame();
        manager.Player.Animator.PlayAllLayers(Animations.NONE);
    }
    public void MoveLeft()
    {
        if (!rightLeg) return;
        progress += step;
        rightLeg = !rightLeg;
    }

    public void MoveRight()
    {
        if(rightLeg) return;
        progress += step;
        rightLeg = !rightLeg;
    }
    protected override void OnUpdate()
    {
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
