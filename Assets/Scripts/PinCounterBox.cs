using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PinCounterBox : MonoBehaviour
{
    public Text standingCountText;
    public float timeThreshold;

    private BowlingBall ball;
    private GameManager gameManager;
    private int standingCount;
    private int previousCount = -1;
    private int lastSettledCount;
    private float lastMoved;
    private float timeSinceMovement;
    private bool ballInBox;
    
    

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        ball = FindObjectOfType<BowlingBall>();
        lastSettledCount = 10;
                
    }

    private void Update()
    {
        UpdateStandingCount();
        if (ballInBox || ball.IsOffLane())
        {
            if (lastMoved == 0f)
            {
                lastMoved = Time.time;
            }
            UpdateLastMovedTime();
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BowlingBall>() != null)
        {
            ballInBox = true;
        }
    }

    private void UpdateStandingCount()
    {
        var pins = FindObjectsOfType<Pin>();
        var standingCount = 0;
        foreach (var pin in pins)
        {
            if (pin != null)
            {
                if (pin.IsPinSet())
                {
                    standingCount++;
                }
            }
        }

        this.standingCount = standingCount;
        if (standingCountText != null)
            standingCountText.text = this.standingCount.ToString();
    }
    
    private void UpdateLastMovedTime()
    {	
        if (previousCount != standingCount)
        {	
            lastMoved = Time.time;
            previousCount = standingCount;
        }
        else
        {
            timeSinceMovement = Time.time - lastMoved;
            if (timeSinceMovement >= timeThreshold)
            {
                PinsHaveSettled();
            }
        }
		
    }
    
    private void PinsHaveSettled()
    {
            previousCount = -1;	
            lastSettledCount = gameManager.HandleEndBowl(lastSettledCount, standingCount);
            ballInBox = false;
    }
}