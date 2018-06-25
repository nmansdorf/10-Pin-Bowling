using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class GameManager : MonoBehaviour
{
	public float CameraResetDelay = 4f;
	public GameObject GameUI;
	public GameObject GameOverPanel;

	private Frame currentFrame;
	private PinsController pinsController;
	private BowlingBall ball;
	private ActionMaster actionMaster = new ActionMaster();
	private CameraControl mCameraControl;
	private ScoreManager scoreManager;
	private const int TOTAL_PINS = 10;
	
	void Start ()
	{
		pinsController = FindObjectOfType<PinsController>();
		ball = FindObjectOfType<BowlingBall>();
		mCameraControl = FindObjectOfType<CameraControl>();
		scoreManager = FindObjectOfType<ScoreManager>();
		Frame.ResetFrames();
	}
	
	public int HandleEndBowl(int lastSettledCount, int standingCount)
	{
		var roll = lastSettledCount - standingCount;
		if (Frame.FrameList.Count < 1 || Frame.FrameList.Last().IsFrameComplete())
		{
			currentFrame = new Frame(roll);
		}
		else
		{
			currentFrame.AddRoll(roll);
		}
		var action = actionMaster.NextAction(currentFrame);
		
		scoreManager.UpdateScoreDisplays();
		ball.ResetBall();
		
		switch (action)
		{
			case ActionMaster.Action.MidFrameReset:
				EndRoll();
				return standingCount;
			case ActionMaster.Action.FrameReset:
				EndFrame();
				return TOTAL_PINS;
			case ActionMaster.Action.EndTurn:
				EndFrame();
				return TOTAL_PINS;
			case ActionMaster.Action.EndGame:
				EndGame();
				return TOTAL_PINS;
			default:
				throw new UnityException("Action not found");
		}
	}

	private void EndGame()
	{
		GameUI.SetActive(false);
		GameOverPanel.SetActive(true);
		SetUpNextShot(CameraResetDelay);
	}

	private void EndFrame()
	{
		pinsController.FrameReset();
		SetUpNextShot(CameraResetDelay);
	}

	private void EndRoll()
	{
		pinsController.MidFrameReset();
		SetUpNextShot(CameraResetDelay);
	}

	private void SetUpNextShot(float time)
	{
		Invoke("ResetCamera",time);
		Invoke("SetReadyToLaunchFlag", time);
	}
	
	private void ResetCamera()
	{
		mCameraControl.ResetCamera();
	}

	private void SetReadyToLaunchFlag()
	{
		ball.SetReadyToLaunch();
	}

	private void ResetScores()
	{
		
		scoreManager.ResetScoresDisplay();
			
	}

	private void ResetScoresInTime(float time)
	{
		Invoke("ResetScores", time);
	}

	public void NewGame()
	{	
		ResetScores();
		Frame.ResetFrames();
		pinsController.FrameReset();
		GameUI.SetActive(true);
		GameOverPanel.SetActive(false);
	}
}
