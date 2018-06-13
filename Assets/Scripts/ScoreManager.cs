using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security.Policy;
using System.Xml;
using System.Xml.Schema;
using NUnit.Framework.Internal.Execution;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{

	public List<FrameScoreDisplay> FrameScoreDisplays;
	public List<int> Scores = new List<int>();
	private int runningTotal;
	private int strikeTotal;
	private int spareTotal;
	private bool spareFlag;

	

	public void UpdateScore(List<Frame> frameList)
	{	
		FrameScoreDisplays[frameList.Count - 1].SetScore(frameList.Last().Pinfalls.Last()); //Update the proper display with a subscore from the last bowl

		CalculateTotalScore(frameList);
			
		if (Scores.Count > 0)
		{
			SetTotalScores();
		}
	}

	private void CalculateTotalScore(List<Frame> frameList)
	{
		Scores.Clear();
		runningTotal = 0;
		
		//handle normal frame scoring
		for (int i = 0; i < frameList.Count; i++)
		{
			if (i <= 9)
			{
				var frame = frameList[i];

				if (frame.IsStrike()) //strike
				{
					CalculateStrikeScore(frameList, i);
				}
				if (frame.FrameComplete() && frame.IsSpare()) //spare
				{
					CalculateSpareScore(frameList, i);
					
				}
				else if(frame.FrameComplete() && !frame.IsSpare())
				{
					runningTotal += frame.FrameScore();
					Scores.Add(runningTotal);
				}
			}
		}
		
		if (frameList.Count == 10)
		{
			CalculateLastFrame(frameList.Last());
		}

		foreach (var score in Scores)
		{
			Debug.Log("Score: " + score);
		}
		
	}

	private void CalculateStrikeScore(List<Frame> frameList, int index)
	{
		spareFlag = true;
		Frame nextFrame = new Frame();
		if (frameList.Count > index + 1)
		{
			nextFrame = frameList[index+1];
		}
		
		Frame nextNextFrame = new Frame();
		if (frameList.Count > index + 2)
		{
			nextNextFrame = frameList[index + 2];
		}
		if (nextFrame.Pinfalls.Count == 1 && nextFrame.Pinfalls.Count() == 10 && index < 9) // not last frame and 2-3 strikes in a row
		{
			runningTotal += 10 + nextFrame.Pinfalls.Count() + nextNextFrame.Pinfalls.First();
			spareFlag = false;
		}
		else if (nextFrame.Pinfalls.Count == 1 && nextFrame.Pinfalls.First() == 10 && index == 9)
		{
			runningTotal += 10 + nextFrame.Pinfalls[0] + nextFrame.Pinfalls[1];
			spareFlag = false;
		}
		else if (nextFrame.Pinfalls.First() == 2)
		{
			runningTotal += 10 + nextFrame.Pinfalls[0] + nextFrame.Pinfalls[1];
			spareFlag = false;
		}
	}

	private void CalculateSpareScore(List<Frame> frameList, int index)
	{
		Frame nextFrame = new Frame();
		if (frameList.Count > index + 1)
		{
			nextFrame = frameList[index+1];
		}
		if (nextFrame.Pinfalls.Count >= 1)
		{
			runningTotal += 10 + nextFrame.Pinfalls.First();
		}

	}

	private void SetTotalScores()
	{
		for (int i = 0; i < Scores.Count; i++)
		{
			FrameScoreDisplays[i].SetTotalScore(Scores[i]);
		}
	}

	private void CalculateLastFrame(Frame lastFrame)
	{
		if (lastFrame.Pinfalls.Count == 2 && !(lastFrame.Pinfalls[0] + lastFrame.Pinfalls[1] >= 10)) //no strike or spare
		{
			runningTotal += lastFrame.Pinfalls[0] + lastFrame.Pinfalls[1];
		}
		else if(lastFrame.Pinfalls.Count> 2 && lastFrame.Pinfalls[0] == 10 && lastFrame.Pinfalls[1] != 10) //strike followed by not strike
		{
			runningTotal += lastFrame.Pinfalls[0] + lastFrame.Pinfalls[1] + lastFrame.Pinfalls[2] + lastFrame.Pinfalls[1] + lastFrame.Pinfalls[2];
		}else if (lastFrame.Pinfalls.Count> 2 && lastFrame.Pinfalls[0] == 10 & lastFrame.Pinfalls[2] == 10) // two strikes or turkey
		{
			runningTotal += lastFrame.Pinfalls[0] + lastFrame.Pinfalls[1] + lastFrame.Pinfalls[2] + lastFrame.Pinfalls[1] + lastFrame.Pinfalls[2] + lastFrame.Pinfalls [2];
		}
		else if (lastFrame.Pinfalls.Count > 2) //spare on the last frame
		{
			runningTotal += 10 + lastFrame.Pinfalls[2] + lastFrame.Pinfalls[2];
		}
		Scores.Add(runningTotal);
	}

	public void ResetScoresInTime(float time)
	{
		Invoke("ResetScores",time);
	}

	private void ResetScores()
	{
		foreach (var frameScoreDisplay in FrameScoreDisplays)
		{
			frameScoreDisplay.ResetScore();
		}
	}
}
