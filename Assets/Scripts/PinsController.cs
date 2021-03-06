﻿using System.Collections.Generic;
using UnityEngine;


public class PinsController : MonoBehaviour
{

	public Pin PinPrefab;
	private PinPosition[] pinPositions;
	private List<Pin> pins;
	private Animator anim;
	private BowlingBall ball;
	
	
	private void Start ()
	{
		ball = FindObjectOfType<BowlingBall>();
		pinPositions = GetComponentsInChildren <PinPosition>();
		pins = new List<Pin>();
		anim = GetComponent<Animator>();
	
		SpawnPins();
		LowerPins();
	}

	public void DebugStrike()
	{
		foreach (var pin in pins)
		{
			if (pin != null)
			{
				pin.GetComponent<Rigidbody>().AddForce(Vector3.right * 200, ForceMode.Impulse);
			}
		}
	}


	private void CleanUpAllPins()
	{
		for (var i = 0; i< pins.Count;)
		{
			if (pins[i] != null)
			{
				Destroy(pins[i].gameObject);
			}
			pins.RemoveAt(i);
		}
	}

	private void CleanUpHitPins()
	{
		for(var i = 0; i<pins.Count; )
		{
			if (pins[i] == null)
			{
				pins.RemoveAt(i);
			}
			else if(!pins[i].IsPinSet())
			{
				Destroy(pins[i].gameObject);
				pins.RemoveAt(i);
			}
			else
			{
				i++;
			}
		}
	}

	private void SpawnPins()
	{
		foreach (var pinPosition in pinPositions)
		{
			var pin = Instantiate(PinPrefab, pinPosition.transform);
			pin.pinPosition = pinPosition;
			pin.transform.position = pin.pinPosition.GetPosition() + pin.InitialPosition;
			pins.Add(pin);
		}
	}


	private void LowerPins()
	{
		foreach (var pin in pins)
		{
			if (pin != null)
			{
				StartCoroutine(pin.MovePinDown(ball.SetReadyToLaunch));
			}
		}
	}

	private void RaisePins()
	{	
		foreach (var pin in pins)
		{
			if (pin != null && pin.IsPinSet())
			{
				StartCoroutine(pin.MovePinUp());
			}
		}
	}

	public void FrameReset()
	{
		anim.SetTrigger("ResetTrigger");
	}

	public void MidFrameReset()
	{
		anim.SetTrigger("MidFrameTrigger");
	}

}
