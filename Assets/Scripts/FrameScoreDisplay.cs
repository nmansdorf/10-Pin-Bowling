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
	private const int STRIKE = 10;
	private int previousScore;

	private void Start()
	{
		ResetScore();
	}

	public void SetFrameScore(int score)
	{
		TotalScore.gameObject.SetActive(true);
		TotalScore.text = score.ToString();
	}

	public void SetRollScore(int score)
	{
		if (!Bowl1Score.IsActive())
		{
			previousScore = -1; //need to skip the spare check on the first bowl of a frame
			SetRollScoreText(score, Bowl1Score);
		}
		else if (!Bowl2Score.IsActive())
		{
			SetRollScoreText(score, Bowl2Score);	
		} else if (TenthFrame && !Bowl3Score.IsActive())
		{
			SetRollScoreText(score, Bowl3Score);
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
		}

		if (!TenthFrame)
		{
			Strike.gameObject.SetActive(false);
			Strike.text = "";
		}		
	}
	
}
