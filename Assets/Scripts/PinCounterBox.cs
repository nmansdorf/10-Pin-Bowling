using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PinCounterBox : MonoBehaviour
{
    public Text _standingCountText;
    public float _timeThreshold;

    private BowlingBall _ball;
    private GameManager _gameManager;
    private int _standingCount;
    private int _previousCount = -1;
    private int _lastSettledCount;
    private float _lastMoved;
    private float _timeSinceMovement;
    private bool _ballInBox;
    
    

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _ball = FindObjectOfType<BowlingBall>();
        _lastSettledCount = 10;
                
    }

    private void Update()
    {
        UpdateStandingCount();
        if (_ballInBox || _ball.IsOffLane())
        {
            if (_lastMoved == 0f)
            {
                _lastMoved = Time.time;
            }
            UpdateLastMovedTime();
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BowlingBall>() != null)
        {
            _ballInBox = true;
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

        _standingCount = standingCount;
        if (_standingCountText != null)
            _standingCountText.text = _standingCount.ToString();
    }
    
    private void UpdateLastMovedTime()
    {	
        if (_previousCount != _standingCount)
        {	
            _lastMoved = Time.time;
            _previousCount = _standingCount;
        }
        else
        {
            _timeSinceMovement = Time.time - _lastMoved;
            if (_timeSinceMovement >= _timeThreshold)
            {
                PinsHaveSettled();
            }
        }
		
    }
    
    private void PinsHaveSettled()
    {
            _previousCount = -1;	
            _lastSettledCount = _gameManager.HandleEndBowl(_lastSettledCount, _standingCount);
            _ballInBox = false;
    }
}