using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.Video;

public class Frame
{
    public static List<Frame> FrameList = new List<Frame>();
    public static List<int> TotalRolls = new List<int>();
    public static List<int?> FrameScores = new List<int?>();
    
    //Frame Rules
    private const int TotalFrames = 10;
    private const int TotalPinsPerFrame = 10;
    private const int MaxRollsPerFrame = 2;
    private const int MaxRollsInFinalFrame = 3;
    private const int StrikeScoreRolls = 2;
    private const int SpareScoreRolls = 1;
    
    //frame variables
    public readonly int frameNumber;
    private int totalRollsIndex; //index of the last roll in the frame relative to total rolls
    private bool lastFrame;
    private List<int> rolls = new List<int>();

    public Frame(int roll)
    {
        FrameList.Add(this);
        frameNumber = FrameList.Count;
        if (frameNumber == TotalFrames)
        {
            lastFrame = true;
        }
        rolls.Add(roll);
        totalRollsIndex = TotalRolls.Count;
        TotalRolls.Add(roll);
        UpdateCumulativeFrameScores();
    }

    private void UpdateCumulativeFrameScores()
    {
        FrameScores.Clear();
        for (int i = 0; i < FrameList.Count; i++)
        {
            var frame = FrameList[i];
            
            if (frame.GetFrameScore() != null)
            {
                if (i == 0)
                {
                    FrameScores.Add(frame.GetFrameScore());
                }
                else
                {
                    FrameScores.Add(frame.GetFrameScore() + FrameScores[i - 1]);
                }
            }
        }
    }

    public List<int> GetRolls()
    {
        return rolls;
    }

    public bool IsFrameComplete()
    {
        //last frame
        if (lastFrame)
        {
            //no extra bowl in last frame
            if (rolls.Count >= MaxRollsPerFrame && !ExtraBowlAwarded())
            {
               
                return true;
            }
            
            return (rolls.Count >= MaxRollsInFinalFrame);
        } 
        
        //strike ends frame
        if (rolls[0] == TotalPinsPerFrame)
        {
            return true;
        }
        
    
        
        return (rolls.Count >= MaxRollsPerFrame);
       
    }

    public void AddRoll(int roll)
    {
        if(!IsFrameComplete())
        {
            rolls.Add(roll);
            totalRollsIndex = TotalRolls.Count;
            TotalRolls.Add(roll);
            UpdateCumulativeFrameScores();
        }
        else
        {
            Debug.LogWarning("Trying to add a roll to a finished frame");
        }
    }

    public int? GetFrameScore()
    {
        if (!IsFrameComplete())
        {
            return null;
        }
        
        var additionalRolls = 0;
        if (IsStrike())
        {
            additionalRolls = StrikeScoreRolls;     
        }
        else if (IsSpare())
        {
            additionalRolls = SpareScoreRolls;
        }

        var score = 0;
        if (additionalRolls > 0)
        {
            //check to see if enough frames bowled
            if (TotalRolls.Count <= totalRollsIndex + additionalRolls)
            {
                return null;
            }
            
            score = 10;
            for (int i = 1; i <= additionalRolls; i++)
            {
                score += TotalRolls[totalRollsIndex + i];
            }
            return score;
        }
        
        return rolls.Sum(); 
    }

    public bool IsStrike()
    {
        return rolls[0] == 10;
    }

    public bool IsSpare()
    {
        return (rolls[0] + rolls[1] == 10);
    }

    private bool ExtraBowlAwarded()
    {
        if (lastFrame)
        {
            //strike or spare
            if (rolls[0] == 10)
            {
                return true;
            }

            if (rolls.Count >= MaxRollsPerFrame 
                && (rolls[0] + rolls[1] == TotalPinsPerFrame))
            {
                return true;
            }
        }
        return false;
    }

    public bool IsLastFrame()
    {
        return lastFrame;
    }

    public static void ResetFrames()
    {
        FrameList.Clear();
        TotalRolls.Clear();
        FrameScores.Clear();
    }
}
