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

	private bool _isMoving;
	private Rigidbody _rigidbody;
	private AudioSource _audioSource;
	private string _translationDirection;
	private const float LANE_BOUNDS = 0.45f;
	private Vector3 GUTTERBALL_BOUNDS = new Vector3(1, -1, 27);
	private bool _readyToLaunch;
	private bool _hasLaunched;
	private bool _ballEnteredBox;




	void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_audioSource = GetComponent<AudioSource>();
		_rigidbody.useGravity = false;
	}

	private void Update()
	{

		if (_translationDirection == "right")
		{
			TranslateBall(Vector3.right);
		}
		else if (_translationDirection == "left")
		{
			TranslateBall(Vector3.left);
		}
	}

	

	public void Launch(Vector3 launchVelocity)
	{
		if (_rigidbody != null && !_isMoving && _readyToLaunch)
		{
			_rigidbody.useGravity = true;
			_rigidbody.velocity = launchVelocity;
			_audioSource.Play();
			_isMoving = true;
			_readyToLaunch = false;
			_hasLaunched = true;
		}
	}
	
	public bool HasLaunched()
	{
		return _hasLaunched;
	}
	
	public void DebugLaunch()
	{
		Launch(Vector3.forward * 8f);	
	}

	public void SetReadyToLaunch( )
	{
		_readyToLaunch = true;
		_hasLaunched = false;
	}

	public bool ReadyToLaunch()
	{
		return _readyToLaunch;
	}

	public bool IsMoving()
	{
		if (_rigidbody.velocity == Vector3.zero)
		{
			_isMoving = false;
		}

		return _isMoving;
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
		_rigidbody.velocity = Vector3.zero;
		_rigidbody.angularVelocity = Vector3.zero;
		_rigidbody.useGravity = false;
		_isMoving = false;
		_ballEnteredBox = false;
	}

	private void TranslateBall(Vector3 direction)
	{
		var position = gameObject.transform.position;
		if(!_isMoving)
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
		return _ballEnteredBox;
	}

	public void TranslateDirection(string direction)
	{
		_translationDirection = direction;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetComponent<PinCounterBox>() != null)
		{
			_ballEnteredBox = true;
		}
	}
}
