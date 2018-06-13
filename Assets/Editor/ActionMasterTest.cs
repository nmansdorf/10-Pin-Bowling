using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class ActionMasterTest
{

	private ActionMaster.Action endTurn = ActionMaster.Action.EndTurn;
	private ActionMaster.Action tidy = ActionMaster.Action.MidFrameReset;
	private ActionMaster.Action reset = ActionMaster.Action.FrameReset;
	private ActionMaster.Action endGame = ActionMaster.Action.EndGame;
	private ActionMaster _actionMaster;
	private GameManager _gameManager;
	
	
	
	[SetUp]
	public void Setup()
	{
		var obj = new GameObject();
		_actionMaster = new ActionMaster();
		obj.AddComponent<GameManager>();
		_gameManager = obj.GetComponent<GameManager>();

	}

	[Test]
	public void PassingTest () {
		Assert.AreEqual (1, 1);
	}

	[Test]
	public void T01OneStrikeReturnsEndTurn()
	{
		_gameManager.FrameList.Clear();
		_gameManager.FrameList.Add(10);
		Assert.AreEqual(endTurn, _actionMaster.Bowl(_gameManager.FrameList));
	}

	[Test]
	public void T02Bowl8ReturnsTidy()
	{
		_gameManager.FrameList.Clear();
		_gameManager.FrameList.Add(8);
		Assert.AreEqual(tidy, _actionMaster.Bowl(_gameManager.FrameList));
	}

	[Test]
	public void T03Bowl2Then8ReturnEndTurn()
	{
		_gameManager.FrameList.Clear();
		_gameManager.FrameList.Add(2);
		_gameManager.FrameList.Add(8);
		Assert.AreEqual(endTurn, _actionMaster.Bowl(_gameManager.FrameList));
			
	}

	[Test]
	public void T04EndGameAfter10ThFrame()
	{
		_gameManager.FrameList.Clear();
		_gameManager.FrameList.Add(1); //1st frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //2nd frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //3rd frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //4th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //5th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //6th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //7th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //8th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //9th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //10th frame
		_gameManager.FrameList.Add(1);
		Assert.AreEqual(endGame, _actionMaster.Bowl(_gameManager.FrameList));
	}

	[Test]
	public void T05AdditionalBowlIfStrikeOnLastFrame()
	{
		_gameManager.FrameList.Clear();
		_gameManager.FrameList.Add(1); //1st frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //2nd frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //3rd frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //4th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //5th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //6th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //7th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //8th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //9th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(10); //10th Frame
		Assert.AreEqual(reset, _actionMaster.Bowl(_gameManager.FrameList)); 
	}

	[Test]
	public void T06EndGameAfter21Bowls()
	{
		_gameManager.FrameList.Clear();	
		Debug.Log(_gameManager.FrameList.Count);
		_gameManager.FrameList.Add(8);//1st frame
		_gameManager.FrameList.Add(1); 
		_gameManager.FrameList.Add(1);//2nd frame
		_gameManager.FrameList.Add(1); 
		_gameManager.FrameList.Add(1);//3rd frame
		_gameManager.FrameList.Add(1); 
		_gameManager.FrameList.Add(1); //4th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1);//5th frame
		_gameManager.FrameList.Add(1); 
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //6th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //7th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //8th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //9th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(10); //10th Frame
		_gameManager.FrameList.Add(1);
		Debug.Log(_gameManager.FrameList.Count);
		Assert.AreEqual(endGame, _actionMaster.Bowl(_gameManager.FrameList));
	}

	[Test]
	public void T07AdditionalBowlIfSpaceOnLastFrame()
	{
		_gameManager.FrameList.Clear();
		_gameManager.FrameList.Add(1); //1st frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //2nd frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //3rd frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //4th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //5th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //6th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //7th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //8th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //9th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(8); //10th Frame
		_gameManager.FrameList.Add(2);
		Assert.AreEqual(reset, _actionMaster.Bowl(_gameManager.FrameList));
	}

	[Test]
	public void T08TidyOnFrame20AfterStrike()
	{
		_gameManager.FrameList.Clear();		
		_gameManager.FrameList.Add(1); //1st frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //2nd frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //3rd frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //4th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //5th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //6th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //7th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //8th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //9th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(10); //10th Frame
		_gameManager.FrameList.Add(5);
		Assert.AreEqual(tidy, _actionMaster.Bowl(_gameManager.FrameList)); 
	}

	[Test]
	public void T09ResetAfter2StrikesOnFrame21()
	{
		_gameManager.FrameList.Clear();
		_gameManager.FrameList.Add(1); //1st frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //2nd frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //3rd frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //4th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //5th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //6th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //7th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //8th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //9th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(10); //10th Frame
		_gameManager.FrameList.Add((10));
		Assert.AreEqual(reset,_actionMaster.Bowl(_gameManager.FrameList));
		
	}
	
	[Test]
	public void T10PinsOnSecondBowlTidyAfterNextRoll()
	{
		_gameManager.FrameList.Clear();	
		_gameManager.FrameList.Add(0);
		_gameManager.FrameList.Add(10);
		_gameManager.FrameList.Add(3);
		Assert.AreEqual(tidy, _actionMaster.Bowl(_gameManager.FrameList));
	}
	
	[Test]
	public void T1110PinsOnSecondBowlEndTun()
	{	
		_gameManager.FrameList.Clear();
		_gameManager.FrameList.Add(0);
		_gameManager.FrameList.Add(10);
		Assert.AreEqual(endTurn, _actionMaster.Bowl(_gameManager.FrameList));
	}

	[Test]
	public void T12EndTurnAfterSpareAndTwoRolls()
	{
		_gameManager.FrameList.Clear();
		_gameManager.FrameList.Add(0);
		_gameManager.FrameList.Add(10);
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(3);
		Assert.AreEqual(endTurn, _actionMaster.Bowl(_gameManager.FrameList));
	}

	[Test]
	public void T13LastFrameTurkey()
	{
		_gameManager.FrameList.Clear();
		_gameManager.FrameList.Add(1); //1st frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //2nd frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //3rd frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //4th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //5th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //6th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //7th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //8th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(1); //9th frame
		_gameManager.FrameList.Add(1);
		_gameManager.FrameList.Add(10); //10th Frame
		_gameManager.FrameList.Add(10);
		_gameManager.FrameList.Add(10);
		Assert.AreEqual(endGame,_actionMaster.Bowl(_gameManager.FrameList));
	}
}