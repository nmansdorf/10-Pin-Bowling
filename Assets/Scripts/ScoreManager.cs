
using System.Collections.Generic;
using UnityEngine;


public class ScoreManager : MonoBehaviour
{

	public List<FrameScoreDisplay> FrameScoreDisplays;
	
	private List<int> scores = new List<int>();
	private List<UnfinishedFrame> unfinishedFrames = new List<UnfinishedFrame>();
	private int previousScore = 0;
	private int bowlIndex;
	private Bowl currentBowl;


	public void UpdateScore(List<Bowl> bowlList)
	{
		bowlIndex = bowlList.Count - 1;
		currentBowl = bowlList[bowlIndex];
		
		var frame = currentBowl.GetFrame();
	
		//Update the proper display with a subscore from the last bowl
		FrameScoreDisplays[frame].SetRollScore(currentBowl.GetScore());

		CalculateTotalScore(bowlList);
		
		if (scores.Count > 0)
		{
			SetTotalScores();
		}
	}

	private void CalculateTotalScore(List<Bowl> bowls)
	{
		

		//strike
		if (currentBowl.GetShotInFrame() == 0 && currentBowl.IsSpareOrStrike())
		{
			CheckForFinishedScores(bowls);
			unfinishedFrames.Add(new UnfinishedFrame(2, bowlIndex));
		}
		else if (currentBowl.GetShotInFrame() == 1)
		{
			var previousBowl = bowls[bowlIndex - 1];
			
			//spare
			if (previousBowl.GetScore() + currentBowl.GetScore() == 10)
			{
				CheckForFinishedScores(bowls);
				unfinishedFrames.Add(new UnfinishedFrame(1, bowlIndex));
			}
			else
			{
				//no strike or spare
				CheckForFinishedScores(bowls);
				var score = previousScore + currentBowl.GetScore() + previousBowl.GetScore();
				scores.Add(score);
				previousScore = score;
				unfinishedFrames.Add(new UnfinishedFrame(0, bowlIndex));
			}
		}
		CheckForFinishedScores(bowls);
	}

	private void CheckForFinishedScores(List<Bowl> bowls)
	{
		for (int i = 0; i < unfinishedFrames.Count; i++)
		{
			var unfinishedFrame = unfinishedFrames[i];
			if (unfinishedFrame.AdditionalBowls != 0)
			{
	
				var extraBowls = unfinishedFrame.AdditionalBowls;
				var bowlIndex = unfinishedFrame.BowlIndex;
				if (unfinishedFrame.BowlIndex + extraBowls < bowls.Count)
				{
					var score = 10;
					for (int j = 0; j < extraBowls; j++)
					{
						score += bowls[bowlIndex + j + 1].GetScore();
					}

					score += previousScore;
					scores.Add(score);
					previousScore = score;
					unfinishedFrame.AdditionalBowls = 0;
				}
			}
		}
	}

	private void SetTotalScores()
	{
		for (int i = 0; i < scores.Count; i++)
		{
			if (unfinishedFrames[i].AdditionalBowls == 0)
			{
				FrameScoreDisplays[i].SetFrameScore(scores[i]);
			}
		}
	}

	public void ResetScores()
	{
		unfinishedFrames.Clear();
		scores.Clear();
		foreach (var frameScoreDisplay in FrameScoreDisplays)
		{
			frameScoreDisplay.ResetScore();
		}
	}

}
