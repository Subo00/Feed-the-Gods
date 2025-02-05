using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MinigameHeadHit : MonoBehaviour, Minigame
{
    [SerializeField] private RectTransform indicator;
    [SerializeField] private float speed = 30f;
    [SerializeField] private float botLimit = -190f;
    [SerializeField] private float topLimit = 190f;
    [SerializeField] private float sweetSpot = 180f;

    private bool goingDown = false;

    void Minigame.DisruptMinigame()
    {
    }

    void Minigame.EndMinigame()
    {
        UpdateManager.Instance.AddUpdatable(this);
    }

    void Minigame.StartMinigame(uint uintValue)
    {
        UpdateManager.Instance.AddUpdatable(this);
    }

    void IMyUpdate.MyUpdate()
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

        if (Input.GetButtonDown("Interact"))
        {

            float temp = Mathf.Abs(sweetSpot - indicator.anchoredPosition.y);
            //add audio 
            
            //add animation
            Debug.Log("temp: " + temp + "\nSweetSpot: " + sweetSpot + "\nindicatorPos: " + indicator.anchoredPosition.y);

            //MinigameManager.Instance.EndMinigame(-1f);


        }
    }
}
