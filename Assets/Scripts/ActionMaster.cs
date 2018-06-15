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
	private Bowl currentBowl;
	private Bowl previousBowl;
	
	public enum Action
	{
		MidFrameReset,
		FrameReset,
		EndTurn,
		EndGame
	};
	
	public Action Bowl(List<Bowl> bowlList)
	{
		var currentBowlIndex = bowlList.Count - 1;
		currentBowl = bowlList[currentBowlIndex];
		
		if (currentBowl.GetScore() < 0 || currentBowl.GetScore() > STRIKE)
		{
			throw new UnityException("Invalid pin count");
		}

		if (bowlList.Count > 1)
		{
			previousBowl = bowlList[currentBowlIndex - 1];
			SetShotCountAndFrameForCurrentBowl();
		}

		if (currentBowl.GetFrame() == 9)
		{
			if (currentBowl.GetShotInFrame() == 2)
			{
				return Action.EndGame;
			}
			if (currentBowl.GetShotInFrame() == 1)
			{
				if (previousBowl.IsSpareOrStrike() && !currentBowl.IsSpareOrStrike())
				{
					return Action.MidFrameReset;
				}
				if (previousBowl.GetScore() + currentBowl.GetScore() >= SPARE)
				{
					return Action.FrameReset;
				}
				return Action.EndGame;
			}
			if (currentBowl.GetShotInFrame() == 0 && currentBowl.IsSpareOrStrike())
			{
				return Action.FrameReset;
			}
		} 
		else if (currentBowl.GetShotInFrame() == 1 || 
		         (currentBowl.GetShotInFrame() == 0 && currentBowl.IsSpareOrStrike()))
		{
			return Action.EndTurn;
		}
		return Action.MidFrameReset;			
	}

	private void SetShotCountAndFrameForCurrentBowl()
	{
		if ((previousBowl.GetShotInFrame() == 1 || previousBowl.IsSpareOrStrike()) 
		    && previousBowl.GetFrame() != 9)
		{
			currentBowl.SetShotInFrame(0);
			currentBowl.SetFrame(previousBowl.GetFrame() + 1);
		}
		else if (previousBowl.GetShotInFrame() == 0 && !previousBowl.IsSpareOrStrike())
		{
			currentBowl.SetShotInFrame(1);
			currentBowl.SetFrame(previousBowl.GetFrame());
		}
		else
		{
			currentBowl.SetShotInFrame(previousBowl.GetShotInFrame() + 1);
			currentBowl.SetFrame(previousBowl.GetFrame());
		}
	}


}