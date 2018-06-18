using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	public List<Bowl> BowlList = new List<Bowl>();
	public float CameraResetDelay = 4f;
	
	private PinController pinController;
	private BowlingBall ball;
	private ActionMaster actionMaster = new ActionMaster();
	private CameraControl mCameraControl;
	private ScoreManager scoreManager;
	private const int TOTAL_PINS = 10;
	
	// Use this for initialization
	void Start ()
	{
		pinController = FindObjectOfType<PinController>();
		ball = FindObjectOfType<BowlingBall>();
		mCameraControl = FindObjectOfType<CameraControl>();
		scoreManager = FindObjectOfType<ScoreManager>();
		BowlList.Clear();

	}
	
	public int HandleEndBowl(int lastSettledCount, int standingCount)
	{
		BowlList.Add(new Bowl(lastSettledCount - standingCount));
		var action = actionMaster.Bowl(BowlList);
		scoreManager.UpdateScore(BowlList);
		
		switch (action)
		{
			case ActionMaster.Action.MidFrameReset:
				pinController.MidFrameReset();
				SetUpNextShot();
				return standingCount;
			case ActionMaster.Action.FrameReset:
				pinController.FrameReset();
				SetUpNextShot();
				return TOTAL_PINS;
			case ActionMaster.Action.EndTurn:
				pinController.FrameReset();
				SetUpNextShot();
				return TOTAL_PINS;
			case ActionMaster.Action.EndGame:
				pinController.FrameReset();
				BowlList.Clear();
				ResetScoresInTime(5f);
				SetUpNextShot(5f);
				return TOTAL_PINS;
			default:
				throw new UnityException("Action not found");
		}
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
		Invoke("ResetScoresInTime", time);
	}
}
