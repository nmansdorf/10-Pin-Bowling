using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;

public class ActionMaster
{

	private List<int> frameList;
	private const int STRIKE = 10;
	private const int SPARE = 10;
	
	public enum Action
	{
		MidFrameReset,
		FrameReset,
		EndTurn,
		EndGame
	};
	
	public Action Bowl(List<int> frameList)
	{
		var lastBowl = frameList.Last();
		
		if (lastBowl < 0 || lastBowl > STRIKE)
		{
			throw new UnityException("Invalid pin count");
		}
		
		if (frameList.Count == 21)
		{
			return Action.EndGame;
		} 
		if(frameList.Count == 20)
		{
			if (frameList[19 - 1] == STRIKE && frameList[20 - 1] < STRIKE)
			{
				return Action.MidFrameReset;
			}
			if (frameList[19 - 1] + frameList[20 - 1] >= SPARE)
			{
				return Action.FrameReset;
			}
			return Action.EndGame;
		}
		
		if( frameList.Count  == 19 && lastBowl == STRIKE) 
		{
			frameList.Add(0);
			return Action.FrameReset;
		}
		
		if (frameList.Count  % 2 == 0)
		{
			return Action.EndTurn;
		}
		if (frameList.Count  % 2 != 0)
		{
			if (lastBowl == STRIKE)
			{
				frameList.Add(0);
				return Action.EndTurn;
			}

			return Action.MidFrameReset;
		} 
		
		throw new UnityException("Not sure what action to return!");
		
	}

}