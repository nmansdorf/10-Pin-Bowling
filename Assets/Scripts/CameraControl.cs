using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
	
	public Vector3 InitialPosition;
	public float CameraOffsetZ = 0f;
	
	private BowlingBall ball;
	

	private void Start()
	{
		ball = FindObjectOfType<BowlingBall>();
	}

	private void Update()
	{
		var ballPos = ball.transform.position;
		var currentPos = gameObject.transform.position;
		
		if (currentPos.z <= 18.29 + CameraOffsetZ && ballPos.z + CameraOffsetZ >= currentPos.z)
		{
			transform.position = new Vector3(currentPos.x, currentPos.y, ballPos.z + CameraOffsetZ );
		}
	}

	public void ResetCamera()
	{
		transform.position = InitialPosition;
	}
}
