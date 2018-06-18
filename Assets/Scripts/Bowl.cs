using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;


public class Bowl
{
    private int score;
    private int frame;
    private int shotInFrame;

    public  Bowl(int score)
    {
        this.score = score;
        frame = 0;
        shotInFrame = 0;
    }

    public bool IsSpareOrStrike()
    {
        return (score == 10);
    }

    public int GetScore()
    {
        return score;
    }

    public int GetFrame()
    {
        return frame;
    }

    public int GetShotInFrame()
    {
        return shotInFrame;
    }

    public void SetShotInFrame(int shot)
    {
        if(shot >= 0 && (frame < 9 && shot <= 2 || frame == 9 && shot <= 3))
        {
            shotInFrame = shot;
        }
        else
        {
            Debug.LogWarning("Shot out of range for the frame");
        }
    }

    public void SetFrame(int frameCount)
    {
        if (frameCount >= 0 && frameCount <= 9)
        {
            frame = frameCount;
        }
        else
        {
            Debug.LogWarning("Frame out of range");
        }
    }

    public bool IsLastFrame()
    {
        return (frame == 9);
    }
}
