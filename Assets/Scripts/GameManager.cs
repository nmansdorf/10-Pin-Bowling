using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	public readonly List<Frame> FrameList = new List<Frame>();
	private PinController _pinController;
	private BowlingBall _ball;
	private ActionMaster _actionMaster = new ActionMaster();
	private CameraControl _mCameraControl;
	private ScoreManager _scoreManager;
	public float cameraResetDelay = 4f;
	private const int TOTAL_PINS = 10;
	
	// Use this for initialization
	void Start ()
	{
		_pinController = FindObjectOfType<PinController>();
		_ball = FindObjectOfType<BowlingBall>();
		_mCameraControl = FindObjectOfType<CameraControl>();
		_scoreManager = FindObjectOfType<ScoreManager>();

	}
	
	public int HandleEndBowl(int lastSettledCount, int standingCount)
	{
		AddBowlToFramelist(lastSettledCount, standingCount);
		return UpdateScoreAndDoNextAction(standingCount);
	}

	private int UpdateScoreAndDoNextAction(int standingCount)
	{
		//_scoreManager.UpdateScore(FrameList);
		var action = _actionMaster.Bowl(FrameList);

		switch (action)
		{
			case ActionMaster.Action.MidFrameReset:
				_pinController.MidFrameReset();
				SetUpNextShot();
				return standingCount;
			case ActionMaster.Action.FrameReset:
				_pinController.FrameReset();
				SetUpNextShot();
				return TOTAL_PINS;
			case ActionMaster.Action.EndTurn:
				_pinController.FrameReset();
				SetUpNextShot();
				return TOTAL_PINS;
			case ActionMaster.Action.EndGame:
				FrameList.Clear();
				_scoreManager.ResetScoresInTime(5f);
				return TOTAL_PINS;
			default:
				throw new UnityException("Action not found");
		}
	}

	private void AddBowlToFramelist(int lastSettledCount, int standingCount)
	{
		var lastFrame = new Frame();
		if (FrameList.Count >= 1)
		{
			lastFrame = FrameList.Last();
			if (!lastFrame.FrameComplete())
			{
				lastFrame.Pinfalls.Add(lastSettledCount - standingCount);
			}
			else
			{
				var newFrame = new Frame();
				newFrame.Pinfalls.Add(lastSettledCount - standingCount);
				FrameList.Add(newFrame);
			}
		}
		else
		{
			var newFrame = new Frame();
			newFrame.Pinfalls.Add(lastSettledCount - standingCount);
			FrameList.Add(newFrame);
		}

		if (FrameList.Count == 10)
		{
			FrameList.Last().SetLastFrame();
		}
	}

	private void SetUpNextShot()
	{
		_ball.ResetBall();
		Invoke("ResetCamera",cameraResetDelay);
		Invoke("SetReadyToLaunchFlag", cameraResetDelay);
	}
	
	private void ResetCamera()
	{
		_mCameraControl.ResetCamera();
	}

	private void SetReadyToLaunchFlag()
	{
		_ball.SetReadyToLaunch();
	}
}
