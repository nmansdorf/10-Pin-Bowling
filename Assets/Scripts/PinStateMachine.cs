using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PinStateMachine : MonoBehaviour
{
	private PinController _pinController;
	private List<bool> stateFlags;
	private List<Action> actionSequence;
	
	
	// Use this for initialization
	void Start ()
	{
		_pinController = GetComponent<PinController>();
	}
	
	// Update is called once per frame
	void Update () {
	
		for (int i = 0; i < stateFlags.Count; i++)
		{
			if (stateFlags[i])
			{
				actionSequence[i]();
				stateFlags[i] = false;
			}
		}
	}

	public void PlayInSequence(List<Action> sequence)
	{
		actionSequence = sequence;
		if (actionSequence != null)
		{
			foreach (var action in actionSequence)
			{
				stateFlags.Add(false);
			}
		}

		stateFlags[0] = true;
	}

	private void coroutineFinished(int sequenceCount)
	{
		stateFlags[sequenceCount + 1] = true;
	}


}
