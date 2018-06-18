using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinPosition : MonoBehaviour
{

	private Vector3 position;

	private void Start()
	{
		position = transform.position;
	}

	public Vector3 GetPosition()
	{
		return position;
	}
}
