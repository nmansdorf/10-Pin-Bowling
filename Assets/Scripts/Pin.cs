﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{

	
	public float PinPositionThreshold = .03f;
	public float RotationThreshold = 3f;
	public float PinTranslationRate = .04f;
	public Vector3 InitialPosition;
	
	private PinPosition pinPosition;
	private Rigidbody pinRigidBody;
	private AudioSource audioSource;
	private Vector3 lastFramePosition;

	private void Start()
	{
		audioSource = gameObject.GetComponent<AudioSource>();
		pinRigidBody = gameObject.GetComponent<Rigidbody>();
	}

	public void SetPinPosition(PinPosition pinPosition)
	{
		this.pinPosition = pinPosition;
	}

	public Vector3 GetPinPosition()
	{
		return pinPosition.GetPosition();
	}
	
	public bool PinAtPinPosition()
	{
		var pinPosition = GetPinPosition();
		var pinCurrentPosition = transform.position;
		return (Mathf.Abs(pinPosition.x - pinCurrentPosition.x) <= PinPositionThreshold &&
		        Mathf.Abs(pinPosition.z - pinCurrentPosition.z) <= PinPositionThreshold);
	}

	public bool IsStanding()
	{
		Vector3 rotationInEulerAngles = transform.rotation.eulerAngles;
		bool withinTiltInXThresholds = (Mathf.Abs(rotationInEulerAngles.x) < RotationThreshold || Mathf.Abs(rotationInEulerAngles.x) > (360 - RotationThreshold));
		bool withinTiltInZThresholds = (Mathf.Abs(rotationInEulerAngles.z) < RotationThreshold || Mathf.Abs(rotationInEulerAngles.z) > (360 - RotationThreshold));
		return (withinTiltInXThresholds && withinTiltInZThresholds);
		
	}

	public bool IsPinSet()
	{
		return (PinAtPinPosition() && IsStanding());
		
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.collider.gameObject.GetComponent<BowlingBall>() != null)
		{
			audioSource.Play();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.GetComponent<PinCounterBox>() != null)
		{
			Destroy(gameObject);
		}
	}
	
	
	public IEnumerator MovePinDown(Action callback = null)
	{
		
		while(gameObject.transform.position.y > GetPinPosition().y + .01f)
		{
			gameObject.transform.position += Vector3.down * PinTranslationRate;
			yield return new WaitForSeconds(.05f);
		}

		if (callback != null)
		{
			callback();
		}

		pinRigidBody.isKinematic = false;
	}

	public IEnumerator MovePinUp()
	{
		pinRigidBody.isKinematic = true;
		gameObject.transform.rotation = Quaternion.identity;

		while (gameObject.transform.position.y <
			   InitialPosition.y)
		{
			gameObject.transform.position += Vector3.up * PinTranslationRate;
			yield return new WaitForSeconds(.05f);
		}	
		
	}
}
