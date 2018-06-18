using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameScoreDisplay : MonoBehaviour
{

	public Text TotalScore;
	public Text Bowl1Score;
	public Text Bowl2Score;
	public Text Bowl3Score;
	public Text Strike;
	public bool TenthFrame;
	private const int STRIKE = 10;
	private int score1;
	private int score2;
	private int score3;

	private void Start()
	{
		ResetScore();
	}

	public void SetTotalScore(int score)
	{
		TotalScore.gameObject.SetActive(true);
		TotalScore.text = score.ToString();
	}

	public void SetScore(int score)
	{
		if (!Bowl1Score.IsActive())
		{
			SetBowl1Score(score);
		}
		else if (!Bowl2Score.IsActive())
		{
			SetBow2Score(score);	
		} else if (TenthFrame && !Bowl3Score.IsActive())
		{
			SetBowl3Score(score);
		}
	}

	private void SetBowl1Score(int score)
	{
		score1 = score;
		if (score1 == 0)
		{
			Bowl1Score.text = "-";
		}
		else if(score == STRIKE && !TenthFrame)
		{
			Strike.gameObject.SetActive(true);
			Strike.text = "X";
		}
		else
		{
			Bowl1Score.gameObject.SetActive(true);
			if (score == STRIKE && TenthFrame)
			{
				Bowl1Score.text = "X";
			}
			else
			{
				Bowl1Score.text = score.ToString();
			}
		}
	}

	private void SetBow2Score(int score)
	{
		score2 = score;
		Bowl2Score.gameObject.SetActive(true);
		if (score2 == 0)
		{
			Bowl2Score.text = "-";
		}
		else if (TenthFrame && score1 == STRIKE && score2 == STRIKE)
		{
			Bowl2Score.text = "X";
		}
		else if (score1 != STRIKE && (score1 + score2 == STRIKE))
		{
			Bowl2Score.text = "/";
		}
		else
		{
			Bowl2Score.text = score.ToString();
		}
	}

	private void SetBowl3Score(int score)
	{
		score3 = score;
		Bowl3Score.gameObject.SetActive(true);
		if (score3 == 0)
		{
			Bowl3Score.text = "-";
		}
		else if (score1 == STRIKE && score2 != STRIKE && (score2 + score3 == STRIKE))
		{
			Bowl3Score.text = "/";
		}
		else if (score2 == STRIKE & score3 == STRIKE)
		{
			Bowl3Score.text = "X";
		}
		else
		{
			Bowl3Score.text = score.ToString();
		}
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
