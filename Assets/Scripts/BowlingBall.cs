using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.UIElements;
using UnityEngine.XR.WSA;

public class BowlingBall : MonoBehaviour
{

	// Use this for initialization
	public Vector3 InitialVelocity = Vector3.forward;
	public Vector3 InitialPosition;
	public float TranslationCoefficient;

	private bool isMoving;
	private Rigidbody ballRigidbody;
	private AudioSource audioSource;
	private string translationDirection;
	private const float LANE_BOUNDS = 0.45f;
	private Vector3 GUTTERBALL_BOUNDS = new Vector3(1, -1, 27);
	private bool readyToLaunch;
	private bool hasLaunched;
	private bool ballEnteredBox;




	void Start()
	{
		ballRigidbody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
		ballRigidbody.useGravity = false;
	}

	private void Update()
	{

		if (translationDirection == "right")
		{
			TranslateBall(Vector3.right);
		}
		else if (translationDirection == "left")
		{
			TranslateBall(Vector3.left);
		}
	}

	

	public void Launch(Vector3 launchVelocity)
	{
		if (ballRigidbody != null && !isMoving && readyToLaunch)
		{
			ballRigidbody.useGravity = true;
			ballRigidbody.velocity = launchVelocity;
			audioSource.Play();
			isMoving = true;
			readyToLaunch = false;
			hasLaunched = true;
		}
	}
	
	public bool HasLaunched()
	{
		return hasLaunched;
	}
	
	public void DebugLaunch()
	{
		Launch(Vector3.forward * 8f);	
	}

	public void SetReadyToLaunch( )
	{
		readyToLaunch = true;
		hasLaunched = false;
	}

	public bool ReadyToLaunch()
	{
		return readyToLaunch;
	}

	public bool IsMoving()
	{
		if (ballRigidbody.velocity == Vector3.zero)
		{
			isMoving = false;
		}

		return isMoving;
	}

	public bool IsOffLane()
	{
		return (transform.position.x > GUTTERBALL_BOUNDS.x ||
		        transform.position.x < -GUTTERBALL_BOUNDS.x ||
		        transform.position.z > GUTTERBALL_BOUNDS.z ||
		        transform.position.y < GUTTERBALL_BOUNDS.y);
	}

	public void ResetBall()
	{
		transform.position = InitialPosition;
		transform.rotation = Quaternion.identity;
		ballRigidbody.velocity = Vector3.zero;
		ballRigidbody.angularVelocity = Vector3.zero;
		ballRigidbody.useGravity = false;
		isMoving = false;
		ballEnteredBox = false;
	}

	private void TranslateBall(Vector3 direction)
	{
		var position = gameObject.transform.position;
		if(!isMoving)
		{
			position += direction * TranslationCoefficient;
			if (position.x >= LANE_BOUNDS)
			{
				position = new  Vector3(LANE_BOUNDS, position.y, position.z);
			} else if (position.x <= -LANE_BOUNDS)
			{
				position = new Vector3(-LANE_BOUNDS, position.y, position.z);
			}

			gameObject.transform.position = position;
		}
	}

	public bool IsInBox()
	{
		return ballEnteredBox;
	}

	public void TranslateDirection(string direction)
	{
		translationDirection = direction;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetComponent<PinCounterBox>() != null)
		{
			ballEnteredBox = true;
		}
	}
}
