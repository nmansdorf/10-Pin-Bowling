using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security.Policy;
using System.Xml;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{

	public List<FrameScoreDisplay> FrameScoreDisplays;
	public List<int> Scores = new List<int>();
	private List<bool> strikeFlags = new List<bool>();
	private bool spareFlag;
	

	public void UpdateScore(List<int> frameList)
	{
		var frame = FrameCalc(frameList.Count); //Get the Frame index
		
		FrameScoreDisplays[frame - 1].SetScore(frameList.Last()); //Update the proper display with a subscore from the last bowl

		if (frameList.Count % 2 == 0)
		{
			CalculateTotalScore(frameList);
		}
		
		if (Scores.Count >= 1)
		{
			SetTotalScores();
		}
	}

	private void CalculateTotalScore(List<int> framelist)
	{
		Scores.Clear();
		//Write the total running score for each frame into a list
		for (int i = 0; i < framelist.Count; i++)
		{
			//What frame are we on
			var frame = FrameCalc(i+1);
		
			//What is the current running score
			var previousScore = 0;
			if(frame > 1 && Scores.Count >= 1)
			{
				previousScore = Scores.Last();
			}
		
			//Condition 1 - strike waits for the next two bowls
			if (i % 2 == 0 && framelist[i] == 10)
			{
				strikeFlags.Add(true);
			}
			//Condition 2 - spare waits for the next bowl
			else if(i % 2 == 1 && framelist[i] + framelist[i - 1] == 10)
			{
				spareFlag = true;
			}
		
			//Condition 3 - display score if no spare or strike after second bowl in the frame
			else if(i % 2 == 1)
			{
				Scores.Add(previousScore + framelist[i] + framelist [i - 1]);
				Debug.Log("Thinks we should put a score in " + Scores.Last());
			}		
			
			if (strikeFlags.Count > 0 && strikeFlags.First() && framelist.Count > i + 2)
			{
				Debug.Log("Thinks there has been a strike");
				var nextBowlScore = framelist[i + 2];
				var secondBowlScore = 0;
				if (framelist[i + 2] == 10)
				{
					secondBowlScore = framelist[i + 4];
					Scores.Add(previousScore + 10 + nextBowlScore + secondBowlScore);
					strikeFlags.RemoveAt(0);		
				}
				else
				{
					secondBowlScore = framelist[i + 3];
					Scores.Add(previousScore + 10 + nextBowlScore + secondBowlScore);
					strikeFlags.RemoveAt(0);	
				}					
			}

			if (spareFlag && framelist.Count > i + 1)
			{
				Scores.Add(previousScore + 10 + framelist[i + 1]);
				Debug.Log("Thinks there has been a spare");
				spareFlag = false;	
			}
		}
	}

	private void SetTotalScores()
	{
		for (int i = 0; i < Scores.Count; i++)
		{
			FrameScoreDisplays[i].SetTotalScore(Scores[i]);
		}
	}

	//returns the frame number based on the number of bowls
	private int FrameCalc(int totalBowls)
	{

		int frame = totalBowls / 2 + totalBowls % 2;
		if (frame > 10)
		{
			frame = 10;
		}

		return frame;
	}

}
