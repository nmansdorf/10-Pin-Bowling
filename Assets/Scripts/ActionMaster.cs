using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;

public class ActionMaster
{

	private const int STRIKE = 10;
	private const int SPARE = 10;
	
	public enum Action
	{
		MidFrameReset,
		FrameReset,
		EndTurn,
		EndGame
	};
	
	public Action Bowl(List<Frame> frameList)
	{
		var lastBowl = frameList.Last().Pinfalls.Last();
		
		if (lastBowl < 0 || lastBowl > STRIKE)
		{
			throw new UnityException("Invalid pin count");
		}

		//10th Frame
		if (frameList.Last().IsLastFrame())
		{
			//3rd bowl of tenth frame
			if (frameList.Last().FrameComplete())
			{
				return Action.EndGame;
			} 
		
			//2nd bowl of tenth frame
			if(frameList.Last().Pinfalls.Count >= 2)
			{
				if (frameList.Last().Pinfalls[0] == STRIKE && frameList.Last().Pinfalls[1] < STRIKE)
				{
					return Action.MidFrameReset;
				}
				if (frameList.Last().IsSpare())
				{
					return Action.FrameReset;
				}
				return Action.EndGame;
			}
			//first bowl of 10th frame
			if(frameList.Last().Pinfalls[0] == 1 && lastBowl == STRIKE)
			{
				return Action.FrameReset;
			}
		}

		if (frameList.Last().FrameComplete())
		{
			return Action.EndTurn;
		}

		if (frameList.Last().Pinfalls[0] != STRIKE)
		{
			return Action.MidFrameReset;
		}


		throw new UnityException("Not sure what action to return!");
	}

}