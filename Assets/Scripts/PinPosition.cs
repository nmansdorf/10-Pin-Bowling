using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinPosition : MonoBehaviour
{

	private Vector3 _position;

	private void Start()
	{
		_position = transform.position;
	}

	public Vector3 GetPosition()
	{
		return _position;
	}
}
