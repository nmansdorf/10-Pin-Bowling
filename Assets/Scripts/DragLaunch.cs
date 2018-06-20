using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BowlingBall))]
public class DragLaunch : MonoBehaviour
{
	public float ScalingFactor = 100f;
	private float timeStart;
	private float timeEnd;
	private Vector2 startPosition;
	private Vector2 endPosition;
	private BowlingBall ball;
	// Use this for initialization
	void Start ()
	{
		ball = GetComponent<BowlingBall>();
	}

	public void DragStart()
	{
		timeStart = Time.time;
		startPosition = Input.mousePosition;

	}

	public void DragEnd()
	{
		endPosition = Input.mousePosition;
		timeEnd = Time.time;
		
		float timeDelta = timeEnd - timeStart;
		
		var x = (endPosition.x - startPosition.x) / (timeDelta * ScalingFactor);
		var y = 0;
		var z = (endPosition.y -  startPosition.y)/(timeDelta * ScalingFactor);
		
		var launchVelocity = new Vector3(x,y,z);
		
		ball.Launch(launchVelocity);
	}
}
