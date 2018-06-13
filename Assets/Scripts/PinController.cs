using System.Collections.Generic;
using UnityEngine;


public class PinController : MonoBehaviour
{

	public Pin PinPrefab;
	private PinPosition[] _pinPositions;
	private List<Pin> _pins;
	private Animator _anim;
	private BowlingBall _ball;
	
	
	// Use this for initialization
	private void Start ()
	{
		_ball = FindObjectOfType<BowlingBall>();
		_pinPositions = GetComponentsInChildren <PinPosition>();
		_pins = new List<Pin>();
		_anim = GetComponent<Animator>();
	
		SpawnPins();
		LowerPins();
	}


	private void CleanUpAllPins()
	{
		for (var i = 0; i< _pins.Count;)
		{
			if (_pins[i] != null)
			{
				Destroy(_pins[i].gameObject);
			}
			_pins.RemoveAt(i);
		}
	}

	private void CleanUpHitPins()
	{
		for(var i = 0; i<_pins.Count; )
		{
			if (_pins[i] == null)
			{
				_pins.RemoveAt(i);
			}
			else if(!_pins[i].PinAtPinPosition())
			{
				Destroy(_pins[i].gameObject);
				_pins.RemoveAt(i);
			}
			else
			{
				i++;
			}
		}
	}

	private void SpawnPins()
	{
		foreach (var pinPosition in _pinPositions)
		{
			var pin = Instantiate(PinPrefab, pinPosition.transform);
			pin.SetPinPosition(pinPosition);
			pin.transform.position = pin.GetPinPosition() + pin.InitialPosition;
			_pins.Add(pin);
		}
	}


	private void LowerPins()
	{
		foreach (var pin in _pins)
		{
			if (pin != null)
			{
				StartCoroutine(pin.MovePinDown(_ball.SetReadyToLaunch));
			}
		}
	}

	private void RaisePins()
	{	
		foreach (var pin in _pins)
		{
			if (pin != null && pin.IsPinSet())
			{
				StartCoroutine(pin.MovePinUp());
			}
		}
	}

	public void FrameReset()
	{
		_anim.SetTrigger("ResetTrigger");
	}

	public void MidFrameReset()
	{
		_anim.SetTrigger("MidFrameTrigger");
	}

}
