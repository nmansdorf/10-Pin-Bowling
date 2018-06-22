using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;

public class ActionMaster
{
	
	public enum Action
	{
		MidFrameReset,
		FrameReset,
		EndTurn,
		EndGame
	};

	public Action NextAction(Frame frame)
	{
		var lastRoll = frame.GetRolls().Last();

		if (lastRoll < 0 || lastRoll > 10)
		{
			throw new UnityException("Invalid pin count");
		}

		if (!frame.IsLastFrame() && frame.IsFrameComplete())
		{
			return Action.EndTurn;
		}
		
		if(frame.IsLastFrame())
		{
			if (frame.IsFrameComplete())
			{
				return Action.EndGame;
			}

			if (frame.IsStrike() || (frame.GetRolls().Count >= 2 && frame.IsSpare()))
			{
				return Action.FrameReset;
			}
		}

		return Action.MidFrameReset;
	}
}