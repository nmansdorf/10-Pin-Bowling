using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;


public class Bowl
{
    private int Score;
    private int Frame;
    private int ShotInFrame;

    public  Bowl(int score)
    {
        Score = score;
        Frame = 0;
        ShotInFrame = 0;
    }

    public bool IsSpareOrStrike()
    {
        return (Score == 10);
    }

    public int GetScore()
    {
        return Score;
    }

    public int GetFrame()
    {
        return Frame;
    }

    public int GetShotInFrame()
    {
        return ShotInFrame;
    }

    public void SetShotInFrame(int shot)
    {
        if(shot >= 0 && (Frame < 9 && shot <= 2 || Frame == 9 && shot <= 3))
        {
            ShotInFrame = shot;
        }
        else
        {
            Debug.LogWarning("Shot out of range for the frame");
        }
    }

    public void SetFrame(int frame)
    {
        if (frame >= 0 && frame <= 9)
        {
            Frame = frame;
        }
        else
        {
            Debug.LogWarning("Frame out of range");
        }
    }

    public bool IsLastFrame()
    {
        return (Frame == 9);
    }
}
