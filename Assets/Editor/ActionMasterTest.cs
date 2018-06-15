using System;
using System.Collections.Generic;
using System.Linq;
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
		_gameManager.BowlList.Clear();
		_gameManager.BowlList.Add(new Bowl(10));
		Assert.AreEqual(endTurn, _actionMaster.Bowl(_gameManager.BowlList));
	}

	[Test]
	public void T02Bowl8ReturnsTidy()
	{
		_gameManager.BowlList.Clear();
		_gameManager.BowlList.Add(new Bowl(8));
		Assert.AreEqual(tidy, _actionMaster.Bowl(_gameManager.BowlList));
	}

	[Test]
	public void T03Bowl2Then8ReturnEndTurn()
	{
		_gameManager.BowlList.Clear();
		_gameManager.BowlList.Add(new Bowl(2));
		_actionMaster.Bowl(_gameManager.BowlList);
		_gameManager.BowlList.Add(new Bowl(8));
		Assert.AreEqual(endTurn, _actionMaster.Bowl(_gameManager.BowlList));
	}

	[Test]
	public void T04EndGameAfter10ThFrame()
	{
		_gameManager.BowlList.Clear();
		for (int i = 0; i < 20; i++)
		{
			_gameManager.BowlList.Add(new Bowl(1));
			_actionMaster.Bowl(_gameManager.BowlList);
		}
		Assert.AreEqual(endGame, _actionMaster.Bowl(_gameManager.BowlList));
	}

	[Test]
	public void T05AdditionalBowlIfStrikeOnLastFrame()
	{
		_gameManager.BowlList.Clear();	
		for (int i = 0; i < 18; i++)
		{
			_gameManager.BowlList.Add(new Bowl(1));
			_actionMaster.Bowl(_gameManager.BowlList);
			Debug.Log("Frame: " + _gameManager.BowlList.Last().GetFrame());
		}
		_gameManager.BowlList.Add(new Bowl(10)); //10th Frame
		Assert.AreEqual(reset, _actionMaster.Bowl(_gameManager.BowlList)); 
	}

	[Test]
	public void T06EndGameAfter21Bowls()
	{
		_gameManager.BowlList.Clear();
		for (int i = 0; i < 19; i++)
		{
			_gameManager.BowlList.Add(new Bowl(1));
			_actionMaster.Bowl(_gameManager.BowlList);
		}
		_gameManager.BowlList.Add(new Bowl(9)); //10th Frame
		_actionMaster.Bowl(_gameManager.BowlList);
		_gameManager.BowlList.Add(new Bowl(1));
		Assert.AreEqual(endGame, _actionMaster.Bowl(_gameManager.BowlList));
	}

	[Test]
	public void T07AdditionalBowlIfSpareOnLastFrame()
	{
		_gameManager.BowlList.Clear();	
		for (int i = 0; i < 18; i++)
		{
			_gameManager.BowlList.Add(new Bowl(1));
			_actionMaster.Bowl(_gameManager.BowlList);
		}
		_gameManager.BowlList.Add(new Bowl(8)); //10th Frame
		_actionMaster.Bowl(_gameManager.BowlList);
		_gameManager.BowlList.Add(new Bowl(2));
		Assert.AreEqual(reset, _actionMaster.Bowl(_gameManager.BowlList));
	}

	[Test]
	public void T08TidyOnFrame20AfterStrike()
	{
		_gameManager.BowlList.Clear();	
		for (int i = 0; i < 18; i++)
		{
			_gameManager.BowlList.Add(new Bowl(1));
			_actionMaster.Bowl(_gameManager.BowlList);
		}
		_gameManager.BowlList.Add(new Bowl(10)); //10th Frame
		_actionMaster.Bowl(_gameManager.BowlList);
		_gameManager.BowlList.Add(new Bowl(5));
		Assert.AreEqual(tidy, _actionMaster.Bowl(_gameManager.BowlList)); 
	}

	[Test]
	public void T09ResetAfter2StrikesOnFrame21()
	{
		_gameManager.BowlList.Clear();	
		for (int i = 0; i < 18; i++)
		{
			_gameManager.BowlList.Add(new Bowl(1));
			_actionMaster.Bowl(_gameManager.BowlList);
		}
		_gameManager.BowlList.Add(new Bowl(10)); //10th Frame
		_actionMaster.Bowl(_gameManager.BowlList);
		_gameManager.BowlList.Add(new Bowl(10));
		Assert.AreEqual(reset,_actionMaster.Bowl(_gameManager.BowlList));
		
	}
	
	[Test]
	public void T10PinsOnSecondBowlTidyAfterNextRoll()
	{
		_gameManager.BowlList.Clear();	
		_gameManager.BowlList.Add(new Bowl(0));
		_actionMaster.Bowl(_gameManager.BowlList);
		_gameManager.BowlList.Add(new Bowl(10));
		_actionMaster.Bowl(_gameManager.BowlList);
		_gameManager.BowlList.Add(new Bowl(3));
		Assert.AreEqual(tidy, _actionMaster.Bowl(_gameManager.BowlList));
	}
	
	[Test]
	public void T1110PinsOnSecondBowlEndTun()
	{	
		_gameManager.BowlList.Clear();
		_gameManager.BowlList.Add(new Bowl(0));
		_actionMaster.Bowl(_gameManager.BowlList);
		_gameManager.BowlList.Add(new Bowl(10));
		Assert.AreEqual(endTurn, _actionMaster.Bowl(_gameManager.BowlList));
	}

	[Test]
	public void T12EndTurnAfterSpareAndTwoRolls()
	{
		_gameManager.BowlList.Clear();
		_gameManager.BowlList.Add(new Bowl(0));
		_actionMaster.Bowl(_gameManager.BowlList);
		_gameManager.BowlList.Add(new Bowl(10));
		_actionMaster.Bowl(_gameManager.BowlList);
		_gameManager.BowlList.Add(new Bowl(1));
		_actionMaster.Bowl(_gameManager.BowlList);
		_gameManager.BowlList.Add(new Bowl(3));
		Assert.AreEqual(endTurn, _actionMaster.Bowl(_gameManager.BowlList));
	}

	[Test]
	public void T13LastFrameTurkey()
	{
		_gameManager.BowlList.Clear();	
		for (int i = 0; i < 18; i++)
		{
			_gameManager.BowlList.Add(new Bowl(1));
			_actionMaster.Bowl(_gameManager.BowlList);
		}
		_gameManager.BowlList.Add(new Bowl(10)); //10th Frame
		_actionMaster.Bowl(_gameManager.BowlList);
		_gameManager.BowlList.Add(new Bowl(10));
		_actionMaster.Bowl(_gameManager.BowlList);
		_gameManager.BowlList.Add(new Bowl(10));
		Assert.AreEqual(endGame,_actionMaster.Bowl(_gameManager.BowlList));
	}
}