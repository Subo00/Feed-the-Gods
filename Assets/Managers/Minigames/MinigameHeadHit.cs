using System.Collections;
using UnityEngine;

public class MinigameHeadHit : Minigame
{
    [SerializeField] private RectTransform indicator;
    [SerializeField] private float speed = 30f;
    [SerializeField] private float botLimit = -190f;
    [SerializeField] private float topLimit = 190f;
    [SerializeField] private float sweetSpot = 170f;

    private bool goingDown = false;


    public override void StartMinigame(uint valueUint, MinigameManager minigameManager = null)
    {
        base.StartMinigame(valueUint, minigameManager);

        manager.Player.ClearInteract();
        manager.Player.SetInteract(OnInteract);
        manager.Player.SetCancle(() => manager.EndMinigame(-1f));
        manager.Player.Animator.PlayAllLayers(Animations.IDLE);
       // manager.Player.Animator.Play(Animations.STOMP, 2, false, false);
    }

    public void OnInteract()
    {
        float temp = Mathf.Abs(sweetSpot - indicator.anchoredPosition.y);
        //add audio 

        //add animation
        Debug.Log("temp: " + temp + "\nindicatorPos: " + indicator.anchoredPosition.y);

        switch(temp)
        {
            case <= 10:
                manager.EndMinigame(1f);
                Debug.Log("1f");
                break;
            case <= 20:
                Debug.Log("0.7f");

                manager.EndMinigame(0.7f);
                break;
            case <= 30:
                Debug.Log("0.5f");

                manager.EndMinigame(0.5f);
                break;
            default:
                Debug.Log("-1f");

                manager.EndMinigame(-1f);
                break;
        }
        //manager.EndMinigame(-1f);

    }

    protected override void OnUpdate()
    {
        //MOVING
        if (goingDown)
        {
            indicator.anchoredPosition += Vector2.down * speed * Time.deltaTime;
        }
        else
        {
            indicator.anchoredPosition += Vector2.up * speed * Time.deltaTime;
        }

        if (indicator.anchoredPosition.y < botLimit)
        {
            goingDown = false;
        }
        else if (indicator.anchoredPosition.y > topLimit)
        {
            goingDown = true;
        }
    }
}
