using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class ActionMasterTest
{

	private ActionMaster.Action endTurn = ActionMaster.Action.EndTurn;
	private ActionMaster.Action midFrameReset = ActionMaster.Action.MidFrameReset;
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
		Frame frame = new Frame();
		frame.Pinfalls.Add(10);
		_gameManager.FrameList.Add(frame);
		Assert.AreEqual(endTurn, _actionMaster.Bowl(_gameManager.FrameList));
	}

	[Test]
	public void T02Bowl8ReturnsTidy()
	{
		_gameManager.FrameList.Clear();
		Frame frame = new Frame();
		frame.Pinfalls.Add(8);
		_gameManager.FrameList.Add(frame);
		Assert.AreEqual(midFrameReset, _actionMaster.Bowl(_gameManager.FrameList));
	}

	[Test]
	public void T03Bowl2Then8ReturnEndTurn()
	{
		_gameManager.FrameList.Clear();
		Frame frame = new Frame();
		frame.Pinfalls.Add(8);
		frame.Pinfalls.Add(2);
		_gameManager.FrameList.Add(frame);
		Assert.AreEqual(endTurn, _actionMaster.Bowl(_gameManager.FrameList));
			
	}

	[Test]
	public void T04EndGameAfter10ThFrame()
	{
		_gameManager.FrameList.Clear();
		for (int i = 0; i <= 9; i++)
		{
			Frame frame = new Frame();
			frame.Pinfalls.Add(1);
			frame.Pinfalls.Add(1);
			_gameManager.FrameList.Add(frame);
		}
		
		Assert.AreEqual(endGame, _actionMaster.Bowl(_gameManager.FrameList));
	}

	[Test]
	public void T05AdditionalBowlIfStrikeOnLastFrame()
	{
		for (int i = 0; i < 9; i++)
		{
			Frame frame = new Frame();
			frame.Pinfalls.Add(1);
			frame.Pinfalls.Add(1);
			_gameManager.FrameList.Add(frame);
		}

		Frame tenthFrame = new Frame();
		tenthFrame.Pinfalls.Add(10);
		_gameManager.FrameList.Add(tenthFrame); 
		Assert.AreEqual(reset, _actionMaster.Bowl(_gameManager.FrameList)); 
	}

	[Test]
	public void T06EndGameAfter21Bowls()
	{
		for (int i = 0; i < 9; i++)
		{
			Frame frame = new Frame();
			frame.Pinfalls.Add(1);
			frame.Pinfalls.Add(1);
			_gameManager.FrameList.Add(frame);
		}
		Frame tenthFrame = new Frame();
		tenthFrame.Pinfalls.Add(10);
		tenthFrame.Pinfalls.Add(1);
		tenthFrame.Pinfalls.Add(1);
		_gameManager.FrameList.Add(tenthFrame); //10th Frame
		Assert.AreEqual(endGame, _actionMaster.Bowl(_gameManager.FrameList));
	}

	[Test]
	public void T07AdditionalBowlIfSpaceOnLastFrame()
	{
		for (int i = 0; i < 9; i++)
		{
			Frame frame = new Frame();
			frame.Pinfalls.Add(1);
			frame.Pinfalls.Add(1);
			_gameManager.FrameList.Add(frame);
		}

		var tenthFrame = new Frame();
		tenthFrame.Pinfalls.Add(8);
		tenthFrame.Pinfalls.Add(2);
		_gameManager.FrameList.Add(tenthFrame); 
		Assert.AreEqual(reset, _actionMaster.Bowl(_gameManager.FrameList));
	}

	[Test]
	public void T08TidyOnFrame20AfterStrike()
	{
		for (int i = 0; i < 9; i++)
		{
			Frame frame = new Frame();
			frame.Pinfalls.Add(1);
			frame.Pinfalls.Add(1);
			_gameManager.FrameList.Add(frame);
		}

		var tenthFrame = new Frame();
		tenthFrame.Pinfalls.Add(10);
		tenthFrame.Pinfalls.Add(5);
		_gameManager.FrameList.Add(tenthFrame); 
		Assert.AreEqual(midFrameReset, _actionMaster.Bowl(_gameManager.FrameList)); 
	}

	[Test]
	public void T09ResetAfter2StrikesOnFrame21()
	{
		for (int i = 0; i < 9; i++)
		{
			Frame frame = new Frame();
			frame.Pinfalls.Add(1);
			frame.Pinfalls.Add(1);
			_gameManager.FrameList.Add(frame);
		}

		Frame tenthFrame = new Frame();
		tenthFrame.Pinfalls.Add(10);
		tenthFrame.Pinfalls.Add(10);
		_gameManager.FrameList.Add(tenthFrame); 
		Assert.AreEqual(reset,_actionMaster.Bowl(_gameManager.FrameList));
		
	}
	
	[Test]
	public void T10PinsOnSecondBowlTidyAfterNextRoll()
	{
		_gameManager.FrameList.Clear();
		Frame firstFrame = new Frame();
		firstFrame.Pinfalls.Add(0);
		firstFrame.Pinfalls.Add(10);

		var secondFrame = new Frame();
		secondFrame.Pinfalls.Add(3);
		_gameManager.FrameList.Add(firstFrame);
		_gameManager.FrameList.Add(secondFrame);
		Assert.AreEqual(midFrameReset, _actionMaster.Bowl(_gameManager.FrameList));
	}
	
	[Test]
	public void T1110PinsOnSecondBowlEndTun()
	{	
		_gameManager.FrameList.Clear();
		var firstFrame = new Frame();
		firstFrame.Pinfalls.Add(0);
		firstFrame.Pinfalls.Add(10);
		_gameManager.FrameList.Add(firstFrame);
		Assert.AreEqual(endTurn, _actionMaster.Bowl(_gameManager.FrameList));
	}

	[Test]
	public void T12EndTurnAfterSpareAndTwoRolls()
	{
		_gameManager.FrameList.Clear();
		var firstFrame = new Frame();
		firstFrame.Pinfalls.Add(0);
		firstFrame.Pinfalls.Add(10);

		var secondFrame = new Frame();
		secondFrame.Pinfalls.Add(1);
		secondFrame.Pinfalls.Add(3);
		_gameManager.FrameList.Add(firstFrame);
		_gameManager.FrameList.Add(secondFrame);
		Assert.AreEqual(endTurn, _actionMaster.Bowl(_gameManager.FrameList));
	}

	[Test]
	public void T13LastFrameTurkey()
	{
		for (int i = 0; i < 9; i++)
		{
			var frame = new Frame();
			frame.Pinfalls.Add(1);
			frame.Pinfalls.Add(1);
			_gameManager.FrameList.Add(frame);
		}

		var tenthFrame = new Frame();
		tenthFrame.Pinfalls.Add(10);
		tenthFrame.Pinfalls.Add(10);
		tenthFrame.Pinfalls.Add(10);
		_gameManager.FrameList.Add(tenthFrame); 
		Assert.AreEqual(endGame,_actionMaster.Bowl(_gameManager.FrameList));
	}
}