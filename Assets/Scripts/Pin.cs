using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{

	
	public float PinPositionThreshold = .03f;
	public float RotationThreshold = 3f;
	public float PinTranslationRate = .04f;
	public Vector3 InitialPosition;
	public PinPosition pinPosition { get; set; }
	
	private Rigidbody pinRigidBody;
	private AudioSource audioSource;

	
	
	private void Start()
	{
		audioSource = gameObject.GetComponent<AudioSource>();
		pinRigidBody = gameObject.GetComponent<Rigidbody>();
	}
	
	public bool PinAtPinPosition()
	{
		var position = pinPosition.GetPosition();
		var pinCurrentPosition = transform.position;
		return (Mathf.Abs(position.x - pinCurrentPosition.x) <= PinPositionThreshold &&
		        Mathf.Abs(position.z - pinCurrentPosition.z) <= PinPositionThreshold);
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
		return (IsStanding());
		
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
		
		while(gameObject.transform.position.y > pinPosition.GetPosition().y + .01f)
		{
			gameObject.transform.position += Vector3.down * PinTranslationRate;
			yield return new WaitForSeconds(.05f);
		}
		
		//callback allows me to know when the pins have completed their movement
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
