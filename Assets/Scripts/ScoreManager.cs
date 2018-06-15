
using System.Collections.Generic;
using UnityEngine;


public class ScoreManager : MonoBehaviour
{

	public List<FrameScoreDisplay> FrameScoreDisplays;
	private List<int> _scores = new List<int>();
	private List<UnfinishedFrame> _unfinishedFrames = new List<UnfinishedFrame>();
	private int _previousScore = 0;

	public void UpdateScore(List<Bowl> bowlList)
	{
		var currentBowlIndex = bowlList.Count - 1;
		var currentBowl = bowlList[currentBowlIndex];
		
		var frame = currentBowl.GetFrame();
	
		//Update the proper display with a subscore from the last bowl
		FrameScoreDisplays[frame].SetScore(currentBowl.GetScore());

		CalculateTotalScore(bowlList);
		
		if (_scores.Count > 0)
		{
			SetTotalScores();
		}
		Debug.Log("previousScore = " + _previousScore);
	}

	public void CalculateTotalScore(List<Bowl> bowls)
	{
		var bowlsIndex = bowls.Count - 1;
		var currentBowl = bowls[bowlsIndex];

		//strike
		if (currentBowl.GetShotInFrame() == 0 && currentBowl.IsSpareOrStrike())
		{
			CheckForFinishedScores(bowls);
			_unfinishedFrames.Add(new UnfinishedFrame(2, bowlsIndex));
		}
		else if (currentBowl.GetShotInFrame() == 1)
		{
			var previousBowl = bowls[bowlsIndex - 1];
			
			//spare
			if (previousBowl.GetScore() + currentBowl.GetScore() == 10)
			{
				CheckForFinishedScores(bowls);
				_unfinishedFrames.Add(new UnfinishedFrame(1, bowlsIndex));
			}
			else
			{
				//no strike or spare
				CheckForFinishedScores(bowls);
				var score = _previousScore + currentBowl.GetScore() + previousBowl.GetScore();
				_scores.Add(score);
				_previousScore = score;
				_unfinishedFrames.Add(new UnfinishedFrame(0, bowlsIndex));
			}
		}
		CheckForFinishedScores(bowls);
	}

	private void CheckForFinishedScores(List<Bowl> bowls)
	{
		for (int i = 0; i < _unfinishedFrames.Count; i++)
		{
			var unfinishedFrame = _unfinishedFrames[i];
			if (unfinishedFrame.AdditionalBowls != 0)
			{
				var score = 0;
				var extraBowls = unfinishedFrame.AdditionalBowls;
				var bowlIndex = unfinishedFrame.BowlIndex;
				if (unfinishedFrame.BowlIndex + extraBowls < bowls.Count)
				{
					score = 10;
					for (int j = 0; j < extraBowls; j++)
					{
						score += bowls[bowlIndex + j + 1].GetScore();
					}

					score += _previousScore;
					_scores.Add(score);
					_previousScore = score;
					unfinishedFrame.AdditionalBowls = 0;
				}
			}
		}
	}

	private void SetTotalScores()
	{
		for (int i = 0; i < _scores.Count; i++)
		{
			if (_unfinishedFrames[i].AdditionalBowls == 0)
			{
				FrameScoreDisplays[i].SetTotalScore(_scores[i]);
			}
		}
	}

	public void ResetScores()
	{
		_unfinishedFrames.Clear();
		_scores.Clear();
	}

}
