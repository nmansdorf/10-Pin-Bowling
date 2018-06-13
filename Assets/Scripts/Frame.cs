using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using NUnit.Framework;
using UnityEngine;

public class Frame
{
	public readonly List<int> Pinfalls = new List<int>();
	private bool _isLastFrame;
	
	public bool IsStrike()
	{
		return !_isLastFrame && Pinfalls[0] == 10;
	}

	public bool IsSpare()
	{
		return Pinfalls[0] != 10 && Pinfalls[0] + Pinfalls[1] == 10;
	}

	public int FrameScore()
	{
		var count = 0;
		for (int i = 0; i < Pinfalls.Count; i++)
		{
			count += Pinfalls[0];
		}

		return count;
	}

	public bool IsLastFrame()
	{
		return _isLastFrame; 
	}

	public void SetLastFrame()
	{
		_isLastFrame = true;
	}

	public bool FrameComplete()
	{
		return (_isLastFrame && Pinfalls.Count == 3 || (!_isLastFrame && Pinfalls.Count == 2) || IsStrike());
	}
}
