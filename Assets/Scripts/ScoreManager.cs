
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ScoreManager : MonoBehaviour
{

	public List<FrameScoreDisplay> FrameScoreDisplays;
	

	private Frame currentFrame;


	public void UpdateScoreDisplays()
	{
		
		//Update the proper display with a subscore
		SetRollScoreDisplays();
		
		if (Frame.FrameScores.Count > 0)
		{
			SetTotalScoresDisplay();
		}
	}

	private void SetRollScoreDisplays()
	{
		var frames = Frame.FrameList;
		for (int i = 0; i < frames.Count; i++)
		{
			var frame = frames[i];
			FrameScoreDisplays[frame.frameNumber].SetRollScore(frame.GetRolls());
		}
	}

	private void SetTotalScoresDisplay()
	{
		var scores = Frame.FrameScores;
		for (int i = 0; i < scores.Count; i++)
		{
			FrameScoreDisplays[i].SetFrameScore(scores[i]);
		}
	}

	public void ResetScoresDisplay()
	{
		foreach (var frameScoreDisplay in FrameScoreDisplays)
		{
			frameScoreDisplay.ResetScore();
		}
	}

}
