using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	public List<Bowl> BowlList = new List<Bowl>();
	
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
		BowlList.Add(new Bowl(lastSettledCount - standingCount));
		var action = _actionMaster.Bowl(BowlList);
		_scoreManager.UpdateScore(BowlList);
		
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
				_pinController.FrameReset();
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
		_ball.ResetBall();
		Invoke("ResetCamera",cameraResetDelay);
		Invoke("SetReadyToLaunchFlag", cameraResetDelay);
	}
	private void SetUpNextShot(float time)
	{
		_ball.ResetBall();
		Invoke("ResetCamera",time);
		Invoke("SetReadyToLaunchFlag", time);
	}
	
	private void ResetCamera()
	{
		_mCameraControl.ResetCamera();
	}

	private void SetReadyToLaunchFlag()
	{
		_ball.SetReadyToLaunch();
	}

	private void ResetScores()
	{
		_scoreManager.ResetScores();
	}

	private void ResetScoresInTime(float time)
	{
		Invoke("ResetScoresInTime", time);
	}
}
