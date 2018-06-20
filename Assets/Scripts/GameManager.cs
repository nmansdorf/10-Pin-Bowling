using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	public List<Bowl> BowlList = new List<Bowl>();
	public float CameraResetDelay = 4f;
	
	private PinsController pinsController;
	private BowlingBall ball;
	private ActionMaster actionMaster = new ActionMaster();
	private CameraControl mCameraControl;
	private ScoreManager scoreManager;
	private const int TOTAL_PINS = 10;
	
	// Use this for initialization
	void Start ()
	{
		pinsController = FindObjectOfType<PinsController>();
		ball = FindObjectOfType<BowlingBall>();
		mCameraControl = FindObjectOfType<CameraControl>();
		scoreManager = FindObjectOfType<ScoreManager>();
	}
	
	public int HandleEndBowl(int lastSettledCount, int standingCount)
	{
		BowlList.Add(new Bowl(lastSettledCount - standingCount));
		var action = actionMaster.Bowl(BowlList);
		scoreManager.UpdateScore(BowlList);
		
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
		pinsController.FrameReset();
		BowlList.Clear();
		ResetScoresInTime(5f);
		SetUpNextShot(5f);
	}

	private void EndFrame()
	{
		pinsController.FrameReset();
		SetUpNextShot();
	}

	private void EndRoll()
	{
		pinsController.MidFrameReset();
		SetUpNextShot();
	}

	private void SetUpNextShot()
	{
		ball.ResetBall();
		Invoke("ResetCamera",CameraResetDelay);
		Invoke("SetReadyToLaunchFlag", CameraResetDelay);
	}
	private void SetUpNextShot(float time)
	{
		ball.ResetBall();
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
		scoreManager.ResetScores();
	}

	private void ResetScoresInTime(float time)
	{
		Invoke("ResetScores", time);
	}
}
