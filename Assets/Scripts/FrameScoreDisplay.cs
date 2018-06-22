using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class FrameScoreDisplay : MonoBehaviour
{

	public Text TotalScore;
	public Text Bowl1Score;
	public Text Bowl2Score;
	public Text Bowl3Score;
	public Text Strike;
	//set on the UI for the 10th frame
	public bool TenthFrame;
	private int previousScore;

	private void Start()
	{
		ResetScore();
	}

	public void SetFrameScore(int? score)
	{
		if (score != null)
		{
			TotalScore.gameObject.SetActive(true);
			TotalScore.text = score.ToString();
		}
	}

	public void SetRollScore(List<int> rolls)
	{
		if (!Bowl1Score.IsActive())
		{
			previousScore = -1; //need to skip the spare check on the first bowl of a frame
			SetRollScoreText(rolls[0], Bowl1Score);
		}
		else if (!Bowl2Score.IsActive())
		{
			SetRollScoreText(rolls[1], Bowl2Score);	
		} else if (TenthFrame && !Bowl3Score.IsActive())
		{
			SetRollScoreText(rolls[2], Bowl3Score);
		}
	}

	private void SetRollScoreText(int score, Text text)
	{
		if (score == 0)
		{
			text.text = "-";
		}
		else if (score + previousScore == 10) //Spare
		{
			text.text = "/";
		}
		else if(score == 10) 				//Strike
		{
			if (TenthFrame)				
			{
				text.text = "X";
			}
			else
			{
				text = Strike;    //Strike on not 10th frame takes up whole roll box
				text.text = "X";			
			}	
		}
		else 								//Open Frame
		{
			text.text = score.ToString();	
		}
		text.gameObject.SetActive(true);
		previousScore = score;
	}

	public void ResetScore()
	{
		TotalScore.gameObject.SetActive(false);
		TotalScore.text = "";
		Bowl1Score.gameObject.SetActive(false);
		Bowl1Score.text = "";
		Bowl2Score.gameObject.SetActive(false);
		Bowl2Score.text = "";
		if (TenthFrame)
		{
			Bowl3Score.gameObject.SetActive(false);
			Bowl3Score.text = "";
			return;
		}
		Strike.gameObject.SetActive(false);
		Strike.text = "";
	}
	
}
