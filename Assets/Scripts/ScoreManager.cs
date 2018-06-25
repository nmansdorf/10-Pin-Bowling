
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{

	public List<FrameScoreDisplay> FrameScoreDisplays;
	public Text FinalScore;

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
		foreach (var frame in frames)
		{
			FrameScoreDisplays[frame.frameNumber-1].SetRollScore(frame.GetRolls());
		}
	}

	private void SetTotalScoresDisplay()
	{
		var scores = Frame.FrameScores;
		var scoreText = "";
		for (int i = 0; i < scores.Count; i++)
		{
			FrameScoreDisplays[i].SetFrameScore(scores[i]);
			if (i == 9)
			{
				SetFinalScoreText();
			}
			scoreText += "Frame " + i + " Score: " + scores[i];
		}	
		Debug.Log(scoreText);
	}

	public void ResetScoresDisplay()
	{
		foreach (var frameScoreDisplay in FrameScoreDisplays)
		{
			frameScoreDisplay.ResetScore();
		}
	}

	private void SetFinalScoreText()
	{
		FinalScore.text = "Final Score: " + Frame.FrameScores.Last();
	}
}
