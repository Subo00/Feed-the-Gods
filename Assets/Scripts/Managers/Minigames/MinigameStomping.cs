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
    private Vector2 inputVector;

    public override void StartMinigame(uint valueUint, MinigameManager minigameManager = null)
    {
        base.StartMinigame(valueUint, minigameManager);
        numToSpawn = valueUint;

        endGoal = calculateFloat(valueUint);
        threshold = calculateFloat(counter);
        Debug.Log("endGoal: " + endGoal);

        manager.Player.SetOnMove(MoveAction);
        manager.Player.AddOnCancle(() => manager.EndMinigame(-1f) );
        manager.Player.Animator.PlayAllLayers(Animations.IDLE);
        manager.Player.Animator.Play(Animations.STOMP, 2, false, false);
    }

    public override void EndMinigame()
    {
        base.EndMinigame();
        manager.Player.Animator.PlayAllLayers(Animations.NONE);
    }
    public void MoveAction(Vector2 input)
    {
        inputVector = input;
    }
    protected override void OnUpdate()
    {
        // Handle player input and update the progress bar
        if (inputVector.x < 0f && rightLeg)
        {
            progress += step;
            rightLeg = !rightLeg;
        }
        if (inputVector.x > 0f && !rightLeg)
        {
            progress += step;
            rightLeg = !rightLeg;
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
