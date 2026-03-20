using UnityEngine;

public class MinigameMixing : Minigame
{
    [SerializeField] private float step = 0.15f;
    [SerializeField] private float arrowSpeed = 0.07f;
    [SerializeField] private float greenLine = 50f;
    [SerializeField] private float redLine = 220f;
    [SerializeField] private float minDuration = 1f;
    [SerializeField] private float maxDuration = 3f;
    [SerializeField] private ProgressBar progressBar;
    [SerializeField] private RectTransform arrow;
    private enum State { UP, RIGHT, DOWN, LEFT }

    private float progress = 0f;
    private float endGoal = 25f;
    private bool goingLeft = false;
    private uint counter = 1;
    private float threshold = 0f;
    private float directionDuration;

    private State currentState;
    private State previousState;

    public override void StartMinigame(uint valueUint, MinigameManager minigameManager = null)
    {
        base.StartMinigame(valueUint, minigameManager);


        currentState = State.UP;
        previousState = State.UP;

        directionDuration = Random.Range(minDuration, maxDuration);
        endGoal = CalculateFloat(valueUint);
        threshold = CalculateFloat(counter);
        Debug.Log("EndGoal " + endGoal);
        Debug.Log("Threshold " + threshold);

        manager.Player.InputHandler.ChangeActionMap(ActionMap.Minigame);
        manager.SetOnLeft(MoveLeft);
        manager.SetOnRight(MoveRight);
        manager.SetOnUp(MoveUp);
        manager.SetOnDown(MoveDown);

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
        previousState = currentState;
        if (previousState == State.UP)
            arrow.anchoredPosition += Vector2.left * step;
        else if (previousState == State.DOWN)
            arrow.anchoredPosition += Vector2.right * step;
        currentState = State.LEFT;
    }

    public void MoveRight()
    {
        previousState = currentState;
        if (previousState == State.DOWN) 
            arrow.anchoredPosition += Vector2.left * step;
        else if (previousState == State.UP)
            arrow.anchoredPosition += Vector2.right * step;
        currentState = State.RIGHT;
    }

    private void MoveUp()
    {
        previousState = currentState;
        if (previousState == State.RIGHT)
            arrow.anchoredPosition += Vector2.left * step;
        else if (previousState == State.LEFT)
            arrow.anchoredPosition += Vector2.right * step;
        currentState = State.UP;
    }

    private void MoveDown()
    {
        previousState = currentState;
        if (previousState == State.LEFT)
            arrow.anchoredPosition += Vector2.left * step;
        else if (previousState == State.RIGHT)
            arrow.anchoredPosition += Vector2.right * step;
        currentState = State.DOWN;
    }

    protected override void OnUpdate()
    {
        IfCases();
        Movement();
    }
    
    private void IfCases()
    {
        if (arrow.anchoredPosition.x >= (greenLine * (-1f)) && arrow.anchoredPosition.x <= greenLine)
            progress += 0.01f;
        if (arrow.anchoredPosition.x <= (redLine * (-1f)) || arrow.anchoredPosition.x >= redLine)
            manager.EndMinigame(-1f);
        if (progress >= endGoal)
            manager.EndMinigame(1f);
        else if (progress >= threshold)
        {
            manager.SpawnFromSpawner();
            counter++;
            threshold = CalculateFloat(counter);
        }
    }

    private void Movement()
    {
        arrow.anchoredPosition += Vector2.right * arrowSpeed * (goingLeft ? (-1f) : 1f);
        progressBar.SetFillAmount(progress/endGoal);

        directionDuration -= Time.deltaTime;
        if (directionDuration <= 0f)
        {
            goingLeft = !goingLeft;
            directionDuration = Random.Range(minDuration, maxDuration);
        }
    }

    private float CalculateFloat(uint value)
    {
        return value * 7.53f;
    }
}
